#!/usr/bin/env python3
"""Normaliza sprite sheet para 2048x256 — padrao Terra Sangrada.

Remove fundo preto por flood-fill das bordas (nao apaga couro/capa escura).
Divide a largura total em 8 frames iguais antes de escalar.

Uso:
  py -3 tools/normalize_sprite_sheet.py entrada.png saida.png
"""

from __future__ import annotations

import sys
from collections import deque
from pathlib import Path

try:
    from PIL import Image
except ImportError:
    print("Instale Pillow: py -3 -m pip install pillow")
    sys.exit(1)

FRAME_COUNT = 8
FRAME_SIZE = 256
SHEET_WIDTH = FRAME_COUNT * FRAME_SIZE
SHEET_HEIGHT = FRAME_SIZE
BG_THRESHOLD = 14


def flood_remove_black_background(image: Image.Image) -> Image.Image:
    rgba = image.convert("RGBA")
    pixels = rgba.load()
    width, height = rgba.size
    visited: set[tuple[int, int]] = set()
    queue: deque[tuple[int, int]] = deque()

    for x in range(width):
        queue.append((x, 0))
        queue.append((x, height - 1))
    for y in range(height):
        queue.append((0, y))
        queue.append((width - 1, y))

    def is_background(x: int, y: int) -> bool:
        red, green, blue, alpha = pixels[x, y]
        return alpha > 0 and red <= BG_THRESHOLD and green <= BG_THRESHOLD and blue <= BG_THRESHOLD

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


def fit_frame(source: Image.Image) -> Image.Image:
    left, top, right, bottom = frame_bounds(source)
    cropped = source.crop((left, top, right, bottom))
    fitted = Image.new("RGBA", (FRAME_SIZE, FRAME_SIZE), (0, 0, 0, 0))
    scale = min(FRAME_SIZE / cropped.width, FRAME_SIZE / cropped.height) * 0.96
    new_width = max(1, int(cropped.width * scale))
    new_height = max(1, int(cropped.height * scale))
    resized = cropped.resize((new_width, new_height), Image.Resampling.LANCZOS)
    offset_x = (FRAME_SIZE - new_width) // 2
    offset_y = FRAME_SIZE - new_height - 4
    fitted.paste(resized, (offset_x, max(0, offset_y)), resized)
    return fitted


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


def frame_fits_with_foot_anchor(
    resized: Image.Image,
    anchor_x: int,
    anchor_y: int,
) -> bool:
    foot_x, foot_y = foot_anchor(resized)
    left, top, right, bottom = frame_bounds(resized)
    paste_x = anchor_x - foot_x
    paste_y = anchor_y - foot_y
    return (
        left + paste_x >= 0
        and right + paste_x <= FRAME_SIZE
        and top + paste_y >= 0
        and bottom + paste_y <= FRAME_SIZE
    )


def fit_frames_with_foot_anchor(crops: list[Image.Image]) -> list[Image.Image]:
    trimmed: list[Image.Image] = []
    max_width = 1
    max_height = 1

    for crop in crops:
        left, top, right, bottom = frame_bounds(crop)
        trimmed.append(crop.crop((left, top, right, bottom)))
        max_width = max(max_width, right - left)
        max_height = max(max_height, bottom - top)

    anchor_x = FRAME_SIZE // 2
    anchor_y = FRAME_SIZE - 4
    scale = min(FRAME_SIZE / max_width, FRAME_SIZE / max_height) * 0.96

    while scale > 0.55:
        resized_frames: list[Image.Image] = []
        for cropped in trimmed:
            new_width = max(1, int(cropped.width * scale))
            new_height = max(1, int(cropped.height * scale))
            resized_frames.append(cropped.resize((new_width, new_height), Image.Resampling.LANCZOS))

        if all(frame_fits_with_foot_anchor(frame, anchor_x, anchor_y) for frame in resized_frames):
            break

        scale -= 0.02

    frames: list[Image.Image] = []
    for resized in resized_frames:
        foot_x, foot_y = foot_anchor(resized)
        canvas = Image.new("RGBA", (FRAME_SIZE, FRAME_SIZE), (0, 0, 0, 0))
        paste_x = anchor_x - foot_x
        paste_y = anchor_y - foot_y
        canvas.paste(resized, (paste_x, paste_y), resized)
        frames.append(canvas)

    return frames


def split_and_compose(image: Image.Image) -> Image.Image:
    cleared = flood_remove_black_background(image)
    width, height = cleared.size

    if should_split_by_content(cleared):
        return compose_frames(split_by_segment_centroids(cleared))

    slice_width = width // FRAME_COUNT
    frames: list[Image.Image] = []

    for index in range(FRAME_COUNT):
        x0 = index * slice_width
        x1 = width if index == FRAME_COUNT - 1 else (index + 1) * slice_width
        frames.append(fit_frame(cleared.crop((x0, 0, x1, height))))

    return compose_frames(frames)


def should_split_by_content(image: Image.Image) -> bool:
    width, height = image.size
    if width != SHEET_WIDTH or height != SHEET_HEIGHT:
        return True

    pixels = image.load()
    bleed_total = 0
    for index in range(FRAME_COUNT):
        x0 = index * FRAME_SIZE
        for y in range(height):
            for x in range(x0, x0 + 8):
                if pixels[x, y][3] > 32:
                    bleed_total += 1
            for x in range(x0 + FRAME_SIZE - 8, x0 + FRAME_SIZE):
                if pixels[x, y][3] > 32:
                    bleed_total += 1

    return bleed_total > 120


def split_by_segment_centroids(image: Image.Image) -> list[Image.Image]:
    left, top, right, bottom = frame_bounds(image)
    content_width = max(FRAME_COUNT, right - left)
    window = content_width // FRAME_COUNT
    pixels = image.load()
    crops: list[Image.Image] = []

    for index in range(FRAME_COUNT):
        segment_left = left + int(index * content_width / FRAME_COUNT)
        segment_right = left + int((index + 1) * content_width / FRAME_COUNT)
        if index == FRAME_COUNT - 1:
            segment_right = right

        weighted_x = 0
        pixel_count = 0
        for x in range(segment_left, segment_right):
            for y in range(top, bottom):
                if pixels[x, y][3] > 32:
                    weighted_x += x
                    pixel_count += 1

        center_x = weighted_x // pixel_count if pixel_count else (segment_left + segment_right) // 2
        crop_left = max(left, center_x - window // 2)
        crop_right = min(right, center_x + window // 2)
        crops.append(image.crop((crop_left, top, crop_right, bottom)))

    return fit_frames_with_foot_anchor(crops)


def split_by_content(image: Image.Image) -> list[Image.Image]:
    left, top, right, bottom = frame_bounds(image)
    content_width = max(FRAME_COUNT, right - left)
    frames: list[Image.Image] = []

    for index in range(FRAME_COUNT):
        x0 = left + int(index * content_width / FRAME_COUNT)
        x1 = left + int((index + 1) * content_width / FRAME_COUNT)
        if index == FRAME_COUNT - 1:
            x1 = right
        frames.append(fit_frame(image.crop((x0, top, x1, bottom))))

    return frames


def compose_frames(frames: list[Image.Image]) -> Image.Image:
    sheet = Image.new("RGBA", (SHEET_WIDTH, SHEET_HEIGHT), (0, 0, 0, 0))
    for index, frame in enumerate(frames):
        sheet.paste(frame, (index * FRAME_SIZE, 0), frame)
    return sheet


def normalize_file(source: Path, destination: Path | None = None) -> Path:
    destination = destination or source.with_name(source.stem + "_normalized.png")
    sheet = split_and_compose(Image.open(source))
    destination.parent.mkdir(parents=True, exist_ok=True)
    sheet.save(destination)

    pixels = sheet.load()
    opaque = sum(1 for y in range(sheet.height) for x in range(sheet.width) if pixels[x, y][3] > 128)
    ratio = 100 * opaque / (sheet.width * sheet.height)
    print(f"OK {source.name} -> {destination.name} opaque={ratio:.1f}%")
    return destination


def main(argv: list[str]) -> int:
    if len(argv) < 2:
        print(__doc__)
        return 1

    for arg in argv[1:]:
        path = Path(arg)
        if not path.exists() or path.suffix.lower() not in {".png", ".webp", ".jpg", ".jpeg"}:
            continue
        normalize_file(path)
    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv))
