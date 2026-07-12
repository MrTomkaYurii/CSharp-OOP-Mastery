import os
import sys
import shutil

sys.path.insert(0, os.path.abspath("."))

import pptx
from pptx.util import Inches

from tools.slides_part1 import build_part1
from tools.slides_part2 import build_part2
from tools.slides_part3 import build_part3

def main():
    print("=" * 60)
    print("STARTING FULL RECREATION OF OOP-10-LINQ (108 SLIDES)")
    print("=" * 60)

    prs = pptx.Presentation()
    prs.slide_width = Inches(13.333)
    prs.slide_height = Inches(7.500)

    # Blank layout is layout index 6 in standard default template
    blank_layout = prs.slide_layouts[6]

    # Build all 3 parts
    build_part1(prs, blank_layout)
    build_part2(prs, blank_layout)
    build_part3(prs, blank_layout)

    slide_count = len(prs.slides)
    print(f"Total slides generated: {slide_count}")
    assert slide_count == 108, f"Error: expected 108 slides, but generated {slide_count}"

    out_dir_1 = "pptx"
    out_dir_2 = os.path.join("101 LINQ Samples", "pptx")
    os.makedirs(out_dir_1, exist_ok=True)
    os.makedirs(out_dir_2, exist_ok=True)

    out_file_1 = os.path.join(out_dir_1, "OOP-10-LINQ.pptx")
    out_file_2 = os.path.join(out_dir_2, "OOP-10-LINQ.pptx")

    print(f"Saving to {out_file_1}...")
    prs.save(out_file_1)

    print(f"Mirroring to {out_file_2}...")
    shutil.copyfile(out_file_1, out_file_2)

    file_size_mb = os.path.getsize(out_file_1) / (1024 * 1024)
    print("=" * 60)
    print(f"SUCCESS: Generated {slide_count} slides!")
    print(f"File size: {file_size_mb:.2f} MB")
    print(f"Saved: {out_file_1}")
    print(f"Saved: {out_file_2}")
    print("=" * 60)

if __name__ == "__main__":
    main()
