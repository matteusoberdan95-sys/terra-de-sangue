#!/usr/bin/env python3
"""Encaixa cada frame 256x256 com margem segura para o tacape nao cortar nas bordas.

Cada frame e ajustado individualmente (sem escala compartilhada).
Uso:
  py -3 tools/fit_sheet_safe_area.py assets/art/sprites/player/
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
MARGIN_LEFT = 22
MARGIN_RIGHT = 14
MARGIN_TOP = 22
MARGIN_BOTTOM = 10


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


def fit_frame(frame: Image.Image) -> Image.Image:
    left, top, right, bottom = frame_bounds(frame)
    cropped = frame.crop((left, top, right, bottom))

    inner_width = FRAME_SIZE - MARGIN_LEFT - MARGIN_RIGHT
    inner_height = FRAME_SIZE - MARGIN_TOP - MARGIN_BOTTOM
    scale = min(inner_width / cropped.width, inner_height / cropped.height, 1.0)
    new_width = max(1, int(cropped.width * scale))
    new_height = max(1, int(cropped.height * scale))
    resized = cropped.resize((new_width, new_height), Image.Resampling.LANCZOS)

    foot_x, foot_y = foot_anchor(resized)
    anchor_x = MARGIN_LEFT + inner_width // 2
    anchor_y = FRAME_SIZE - MARGIN_BOTTOM

    canvas = Image.new("RGBA", (FRAME_SIZE, FRAME_SIZE), (0, 0, 0, 0))
    paste_x = anchor_x - foot_x
    paste_y = anchor_y - foot_y

    # Garante margem minima sem estourar o lado oposto.
    bounds_left, bounds_top, bounds_right, bounds_bottom = frame_bounds(resized)
    paste_x = max(paste_x, MARGIN_LEFT - bounds_left)
    paste_x = min(paste_x, FRAME_SIZE - MARGIN_RIGHT - bounds_right)
    paste_y = max(paste_y, MARGIN_TOP - bounds_top)
    paste_y = min(paste_y, FRAME_SIZE - MARGIN_BOTTOM - bounds_bottom)

    canvas.paste(resized, (paste_x, paste_y), resized)
    return canvas


def process_sheet(path: Path) -> None:
    source = Image.open(path).convert("RGBA")
    if source.size != (FRAME_COUNT * FRAME_SIZE, FRAME_SIZE):
        raise ValueError(f"{path.name}: esperado 2048x256, veio {source.size}")

    output = Image.new("RGBA", source.size, (0, 0, 0, 0))
    for index in range(FRAME_COUNT):
        frame = source.crop((index * FRAME_SIZE, 0, (index + 1) * FRAME_SIZE, FRAME_SIZE))
        output.paste(fit_frame(frame), (index * FRAME_SIZE, 0))

    backup = path.with_name(path.stem + "_before_safe_area.png")
    if not backup.exists():
        source.save(backup)

    output.save(path)
    print(f"OK {path.name}")


def main(argv: list[str]) -> int:
    if len(argv) < 2:
        print(__doc__)
        return 1

    for arg in argv[1:]:
        path = Path(arg)
        if path.is_dir():
            for child in sorted(path.glob("arandu_*_sheet.png")):
                if "_before_safe_area" in child.name:
                    continue
                process_sheet(child)
            continue
        if path.exists() and path.suffix.lower() == ".png":
            process_sheet(path)
    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv))
