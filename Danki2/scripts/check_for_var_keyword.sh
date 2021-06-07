# Searches all script files for the var keyword. Files and
# directories can be excluded with the VAR_FILES_TO_EXCLUDE
# and VAR_DIRECTORIES_TO_EXCLUDE environment varialbles. If
# the keyword is found then the script fails.

eval grep var -wr Danki2/Assets/Scripts \
  --exclude="$VAR_FILES_TO_EXCLUDE" \
  --exclude-dir="$VAR_DIRECTORIES_TO_EXCLUDE"

if (($? == 1)); then
  Found no instances of the var keyword, continuing build.
  exit 0
fi
echo Found instances of the var keyword, aborting build.
exit 1
