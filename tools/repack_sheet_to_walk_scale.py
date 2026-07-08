#!/usr/bin/env python3
"""Reempacota sheet irregular para 2048x256 com escala alinhada ao walk de referencia.

Cada frame e ajustado individualmente (sem escala compartilhada que encolhe a corrida).
Uso:
  py -3 tools/repack_sheet_to_walk_scale.py referencia_walk.png alvo.png [saida.png]
"""

from __future__ import annotations

import sys
from pathlib import Path

try:
    from PIL import Image
except ImportError:
    print("Instale Pillow: py -3 -m pip install pillow")
    sys.exit(1)

FRAME_COUNT = 8
FRAME_SIZE = 256
SHEET_WIDTH = FRAME_COUNT * FRAME_SIZE
MARGIN_LEFT = 28
MARGIN_RIGHT = 10
MARGIN_TOP = 18
MARGIN_BOTTOM = 8
HEIGHT_SCALE = 0.94


def flood_remove_black_background(image: Image.Image) -> Image.Image:
    from collections import deque

    rgba = image.convert("RGBA")
    pixels = rgba.load()
    width, height = rgba.size
    visited: set[tuple[int, int]] = set()
    queue: deque[tuple[int, int]] = deque()
    threshold = 14

    for x in range(width):
        queue.append((x, 0))
        queue.append((x, height - 1))
    for y in range(height):
        queue.append((0, y))
        queue.append((width - 1, y))

    def is_background(x: int, y: int) -> bool:
        red, green, blue, alpha = pixels[x, y]
        return alpha > 0 and red <= threshold and green <= threshold and blue <= threshold

    while queue:
        x, y = queue.popleft()
        if (x, y) in visited or x < 0 or y < 0 or x >= width or y >= height:
            continue
        if not is_background(x, y):
            continue
        visited.add((x, y))
        pixels[x, y] = (0, 0, 0, 0)
        queue.extend([(x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1)])

    return rgba


def frame_bounds(image: Image.Image) -> tuple[int, int, int, int]:
    pixels = image.load()
    width, height = image.size
    min_x, min_y = width, height
    max_x, max_y = 0, 0
    found = False

    for y in range(height):
        for x in range(width):
            if pixels[x, y][3] < 16:
                continue
            found = True
            min_x = min(min_x, x)
            min_y = min(min_y, y)
            max_x = max(max_x, x)
            max_y = max(max_y, y)

    if not found:
        return 0, 0, width, height
    return min_x, min_y, max_x + 1, max_y + 1


def foot_anchor(image: Image.Image) -> tuple[int, int]:
    left, top, right, bottom = frame_bounds(image)
    pixels = image.load()
    foot_y = bottom - 1
    min_x = right
    max_x = left

    for y in range(max(top, bottom - 4), bottom):
        for x in range(left, right):
            if pixels[x, y][3] > 32:
                foot_y = y
                min_x = min(min_x, x)
                max_x = max(max_x, x)

    if min_x <= max_x:
        return (min_x + max_x) // 2, foot_y

    return (left + right) // 2, bottom - 1


def median_content_height(sheet: Image.Image) -> float:
    heights: list[int] = []
    for index in range(FRAME_COUNT):
        frame = sheet.crop((index * FRAME_SIZE, 0, (index + 1) * FRAME_SIZE, FRAME_SIZE))
        left, top, right, bottom = frame_bounds(frame)
        heights.append(bottom - top)
    return sum(heights) / len(heights)


def split_raw_frames(image: Image.Image) -> list[Image.Image]:
    cleared = flood_remove_black_background(image)
    width, height = cleared.size
    slice_width = max(1, width // FRAME_COUNT)
    frames: list[Image.Image] = []

    for index in range(FRAME_COUNT):
        x0 = index * slice_width
        x1 = width if index == FRAME_COUNT - 1 else min(width, (index + 1) * slice_width)
        if x0 >= x1:
            x0 = max(0, width - slice_width)
            x1 = width
        frames.append(cleared.crop((x0, 0, x1, height)))

    return frames


def fit_to_target(crop: Image.Image, target_height: float) -> Image.Image:
    left, top, right, bottom = frame_bounds(crop)
    trimmed = crop.crop((left, top, right, bottom))

    inner_width = FRAME_SIZE - MARGIN_LEFT - MARGIN_RIGHT
    inner_height = FRAME_SIZE - MARGIN_TOP - MARGIN_BOTTOM
    scale = min(
        target_height / trimmed.height,
        inner_width / trimmed.width,
        inner_height / trimmed.height,
    ) * HEIGHT_SCALE
    new_width = max(1, int(trimmed.width * scale))
    new_height = max(1, int(trimmed.height * scale))
    resized = trimmed.resize((new_width, new_height), Image.Resampling.LANCZOS)

    foot_x, foot_y = foot_anchor(resized)
    anchor_x = MARGIN_LEFT + inner_width // 2
    anchor_y = FRAME_SIZE - MARGIN_BOTTOM
    canvas = Image.new("RGBA", (FRAME_SIZE, FRAME_SIZE), (0, 0, 0, 0))
    paste_x = anchor_x - foot_x
    paste_y = anchor_y - foot_y

    bounds_left, bounds_top, bounds_right, bounds_bottom = frame_bounds(resized)
    paste_x = max(paste_x, MARGIN_LEFT - bounds_left)
    paste_x = min(paste_x, FRAME_SIZE - MARGIN_RIGHT - bounds_right)
    paste_y = max(paste_y, MARGIN_TOP - bounds_top)
    paste_y = min(paste_y, FRAME_SIZE - MARGIN_BOTTOM - bounds_bottom)

    canvas.paste(resized, (paste_x, paste_y), resized)
    return canvas


def repack(reference_walk: Path, source: Path, destination: Path) -> None:
    walk = Image.open(reference_walk).convert("RGBA")
    if walk.size != (SHEET_WIDTH, FRAME_SIZE):
        raise ValueError(f"Walk referencia deve ser 2048x256, veio {walk.size}")

    target_height = median_content_height(walk)
    raw_frames = split_raw_frames(Image.open(source))
    sheet = Image.new("RGBA", (SHEET_WIDTH, FRAME_SIZE), (0, 0, 0, 0))

    for index, raw in enumerate(raw_frames):
        sheet.paste(fit_to_target(raw, target_height), (index * FRAME_SIZE, 0))

    destination.parent.mkdir(parents=True, exist_ok=True)
    sheet.save(destination)

    heights = []
    for index in range(FRAME_COUNT):
        frame = sheet.crop((index * FRAME_SIZE, 0, (index + 1) * FRAME_SIZE, FRAME_SIZE))
        left, top, right, bottom = frame_bounds(frame)
        heights.append(bottom - top)
    print(
        f"OK {source.name} -> {destination.name} "
        f"target_h={target_height:.0f} result_h={heights} avg={sum(heights)/len(heights):.0f}"
    )


def main(argv: list[str]) -> int:
    if len(argv) < 3:
        print(__doc__)
        return 1

    reference = Path(argv[1])
    for source in argv[2:]:
        source_path = Path(source)
        destination = (
            Path(argv[3])
            if len(argv) > 3 and not Path(argv[3]).suffix == ".png"
            else source_path.with_name(source_path.stem + "_walkscaled.png")
        )
        if len(argv) > 3 and Path(argv[3]).suffix == ".png" and source == argv[-2]:
            destination = Path(argv[3])
        # simpler: reference, source, optional dest
        out = (
            Path(argv[3])
            if len(argv) >= 4 and argv[3].endswith(".png") and source_path == Path(argv[2])
            else source_path.with_name(source_path.stem + "_walkscaled.png")
        )
        if source_path == Path(argv[2]) and len(argv) >= 4:
            out = Path(argv[3]) if argv[3].endswith(".png") else out
        repack(reference, source_path, out if source_path == Path(argv[2]) else source_path.with_suffix(".walkscaled.png"))
    return 0


if __name__ == "__main__":
    if len(sys.argv) < 3:
        print(__doc__)
        raise SystemExit(1)

    reference = Path(sys.argv[1])
    source = Path(sys.argv[2])
    destination = Path(sys.argv[3]) if len(sys.argv) > 3 else source.with_name(source.stem + "_walkscaled.png")
    repack(reference, source, destination)
