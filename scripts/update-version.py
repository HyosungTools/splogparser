#!/usr/bin/env python3
import re
from pathlib import Path

VERSION_FILE = Path("VERSION")
VERSION_CS_FILE = Path("src/Version.cs")

def update_version(new_version: str):
    VERSION_FILE.write_text(new_version)

    cs_content = f"""// Auto-generated version file
namespace SplogParser
{{
    public static class VersionInfo
    {{
        public const string Current = "{new_version}";
    }}
}}
"""
    VERSION_CS_FILE.write_text(cs_content)

if __name__ == "__main__":
    import sys
    if len(sys.argv) != 2:
        print("Usage: python update-version.py <version>")
        sys.exit(1)
    update_version(sys.argv[1])
