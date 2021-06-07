eval grep var -wr Danki2/Assets/Scripts \
  --exclude="$FILES_TO_EXCLUDE" \
  --exclude-dir="$DIRECTORIES_TO_EXCLUDE"

if (($? == 1)); then
  exit 0
fi
exit 1
