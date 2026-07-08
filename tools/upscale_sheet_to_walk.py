#!/usr/bin/env python3
"""Escala cada frame de um sheet 2048x256 para a altura do walk de referencia."""

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
MARGIN_LEFT = 24
MARGIN_RIGHT = 12
MARGIN_TOP = 16
MARGIN_BOTTOM = 8
HEIGHT_SCALE = 0.96
MIN_EDGE_MARGIN = 18


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


def fit_frame(
    frame: Image.Image,
    target_height: float,
    extra_left_margin: int = 0,
    enforce_margin: bool = False,
) -> Image.Image:
    left, top, right, bottom = frame_bounds(frame)
    trimmed = frame.crop((left, top, right, bottom))

    margin_left = MARGIN_LEFT + extra_left_margin
    inner_width = FRAME_SIZE - margin_left - MARGIN_RIGHT
    inner_height = FRAME_SIZE - MARGIN_TOP - MARGIN_BOTTOM
    scale = min(
        target_height / trimmed.height,
        inner_width / trimmed.width,
        inner_height / trimmed.height,
    ) * HEIGHT_SCALE

    while scale > 0.5:
        new_width = max(1, int(trimmed.width * scale))
        new_height = max(1, int(trimmed.height * scale))
        resized = trimmed.resize((new_width, new_height), Image.Resampling.LANCZOS)

        foot_x, foot_y = foot_anchor(resized)
        anchor_x = margin_left + inner_width // 2
        anchor_y = FRAME_SIZE - MARGIN_BOTTOM
        paste_x = anchor_x - foot_x
        paste_y = anchor_y - foot_y

        bounds_left, bounds_top, bounds_right, bounds_bottom = frame_bounds(resized)
        paste_x = max(paste_x, margin_left - bounds_left)
        paste_x = min(paste_x, FRAME_SIZE - MARGIN_RIGHT - bounds_right)
        paste_y = max(paste_y, MARGIN_TOP - bounds_top)
        paste_y = min(paste_y, FRAME_SIZE - MARGIN_BOTTOM - bounds_bottom)

        if not enforce_margin:
            canvas = Image.new("RGBA", (FRAME_SIZE, FRAME_SIZE), (0, 0, 0, 0))
            canvas.paste(resized, (paste_x, paste_y), resized)
            return canvas

        left_edge = bounds_left + paste_x
        right_edge = bounds_right + paste_x
        top_edge = bounds_top + paste_y
        bottom_edge = bounds_bottom + paste_y
        if (
            left_edge >= MIN_EDGE_MARGIN
            and right_edge <= FRAME_SIZE - MIN_EDGE_MARGIN
            and top_edge >= MIN_EDGE_MARGIN
            and bottom_edge <= FRAME_SIZE - MIN_EDGE_MARGIN
        ):
            canvas = Image.new("RGBA", (FRAME_SIZE, FRAME_SIZE), (0, 0, 0, 0))
            canvas.paste(resized, (paste_x, paste_y), resized)
            return canvas

        scale -= 0.02

    return frame


def upscale_sheet(
    reference_walk: Path,
    source_sheet: Path,
    destination: Path,
    extra_left_margin: int = 0,
    enforce_margin: bool = False,
    height_multiplier: float = 1.0,
) -> None:
    walk = Image.open(reference_walk).convert("RGBA")
    source = Image.open(source_sheet).convert("RGBA")
    if walk.size != (SHEET_WIDTH, FRAME_SIZE):
        raise ValueError(f"Walk referencia deve ser 2048x256, veio {walk.size}")
    if source.size != (SHEET_WIDTH, FRAME_SIZE):
        raise ValueError(f"Sheet fonte deve ser 2048x256, veio {source.size}")

    target_height = median_content_height(walk) * height_multiplier
    output = Image.new("RGBA", (SHEET_WIDTH, FRAME_SIZE), (0, 0, 0, 0))
    heights: list[int] = []

    for index in range(FRAME_COUNT):
        frame = source.crop((index * FRAME_SIZE, 0, (index + 1) * FRAME_SIZE, FRAME_SIZE))
        fitted = fit_frame(frame, target_height, extra_left_margin, enforce_margin)
        output.paste(fitted, (index * FRAME_SIZE, 0))
        left, top, right, bottom = frame_bounds(fitted)
        heights.append(bottom - top)

    destination.parent.mkdir(parents=True, exist_ok=True)
    output.save(destination)
    print(
        f"OK {source_sheet.name} -> {destination.name} "
        f"target_h={target_height:.0f} result={heights} avg={sum(heights)/len(heights):.0f}"
    )


if __name__ == "__main__":
    if len(sys.argv) < 4:
        print(
            "Uso: py -3 tools/upscale_sheet_to_walk.py walk.png sheet.png saida.png "
            "[margem_esquerda_extra] [enforce_margin=0|1] [height_multiplier]"
        )
        raise SystemExit(1)

    extra = int(sys.argv[4]) if len(sys.argv) > 4 else 0
    enforce = bool(int(sys.argv[5])) if len(sys.argv) > 5 else False
    height_mult = float(sys.argv[6]) if len(sys.argv) > 6 else 1.0
    upscale_sheet(Path(sys.argv[1]), Path(sys.argv[2]), Path(sys.argv[3]), extra, enforce, height_mult)
