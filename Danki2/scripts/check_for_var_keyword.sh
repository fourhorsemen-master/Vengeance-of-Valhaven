eval grep var -wr Danki2/Assets/Scripts \
  --exclude="$VAR_FILES_TO_EXCLUDE" \
  --exclude-dir="$VAR_DIRECTORIES_TO_EXCLUDE"

if (($? == 1)); then
  Found no instances of the var keyword, continuing build.
  exit 0
fi
echo Found instances of the var keyword, aborting build.
exit 1
