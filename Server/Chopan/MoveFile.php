<?php
$user = $_GET['username'] ?? '';
$pass = $_GET['password'] ?? '';
$src = urldecode($_GET['FilePath'] ?? '');
$dst = urldecode($_GET['FileNewPath'] ?? '');
$base = "C:/Chopan/$user";
$ini = "$base/Properties.ini";
$filesRoot = "$base/Files";

if (!is_dir($base) || !file_exists($ini)) exit;
$props = parse_ini_file($ini);
if (isset($props['locked']) && $props['locked'] == '1') exit;
if (!isset($props['password']) || $props['password'] !== $pass) exit;

$srcFile = realpath($filesRoot . $src);
$dstFile = $filesRoot . $dst;
if (!$srcFile || strpos($srcFile, realpath($filesRoot)) !== 0 || !is_file($srcFile)) exit;

$dstDir = dirname($dstFile);
if (!is_dir($dstDir)) mkdir($dstDir, 0777, true);

if (rename($srcFile, $dstFile)) {
    echo "Success";
}
?>