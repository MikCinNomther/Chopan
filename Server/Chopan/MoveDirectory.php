<?php
$user = $_GET['username'] ?? '';
$pass = $_GET['password'] ?? '';
$srcPath = urldecode($_GET['SourceDirectoryPath'] ?? '');
$dstPath = urldecode($_GET['DirectoryNewPath'] ?? '');
$base = "C:/Chopan/$user";
$ini = "$base/Properties.ini";
$filesRoot = "$base/Files";

// 驗證用戶
if (!is_dir($base) || !file_exists($ini)) exit;
$props = parse_ini_file($ini);
if (isset($props['locked']) && $props['locked'] == '1') exit;
if (!isset($props['password']) || $props['password'] !== $pass) exit;

// 處理路徑，保證只有一個斜線
$srcPath = ltrim(str_replace('\\', '/', $srcPath), '/');
$dstPath = ltrim(str_replace('\\', '/', $dstPath), '/');

$srcDir = realpath($filesRoot . '/' . $srcPath);
$dstDir = $filesRoot . '/' . $dstPath;

// 檢查來源目錄合法性
if ($srcDir === false || strpos($srcDir, realpath($filesRoot)) !== 0 || !is_dir($srcDir)) exit;

// 檢查目標目錄合法性
$dstParent = dirname($dstDir);
if (strpos(realpath($dstParent) ?: $dstParent, realpath($filesRoot)) !== 0) exit;

// 建立目標父目錄
if (!is_dir($dstParent)) mkdir($dstParent, 0777, true);

// 執行移動
if (rename($srcDir, $dstDir)) {
    echo "Success";
} else {
    echo "MoveError";
}
?>