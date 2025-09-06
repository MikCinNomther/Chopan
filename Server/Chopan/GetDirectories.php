<?php
$user = $_GET['username'] ?? '';
$pass = $_GET['password'] ?? '';
$root = urldecode($_GET['DirectoryRoot'] ?? '/');
$base = "C:/Chopan/$user";
$ini = "$base/Properties.ini";
$filesRoot = "$base/Files";

if (!is_dir($base) || !file_exists($ini)) exit;
$props = parse_ini_file($ini);
if (isset($props['locked']) && $props['locked'] == '1') exit;
if (!isset($props['password']) || $props['password'] !== $pass) exit;

$dir = realpath($filesRoot . $root);
if (!$dir || strpos($dir, realpath($filesRoot)) !== 0) exit; // 防止目錄穿越

$dirs = [];
foreach (glob($dir . '/*', GLOB_ONLYDIR) as $d) {
    $dirs[] = basename($d);
}
echo implode("\r\n", $dirs);
?>