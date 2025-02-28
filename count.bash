git log --shortstat --no-merges --author $1 | awk '
    BEGIN { files = 0; insertions = 0; deletions = 0 }
    /files? changed/ {
        split($0, parts, ",");
        for (i in parts) {
            sub(/^ +/, "", parts[i]);  # 去除前导空格
            if (parts[i] ~ /files? changed/) {
                sub(/[^0-9]*/, "", parts[i]);
                files += parts[i] + 0;
            }
            else if (index(parts[i], "insertions(+)")) {
                sub(/[^0-9]*/, "", parts[i]);
                insertions += parts[i] + 0;
            }
            else if (index(parts[i], "deletions(-)")) {
                sub(/[^0-9]*/, "", parts[i]);
                deletions += parts[i] + 0;
            }
        }
    }
    END {
        printf "总文件变更次数: %d\n总插入行数: %d\n总删除行数: %d\n", files, insertions, deletions
    }
'