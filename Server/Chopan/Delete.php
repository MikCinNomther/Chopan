<?php
$user = $_GET['username'] ?? '';
$pass = $_GET['password'] ?? '';
$path = urldecode($_GET['FilePath'] ?? '');
$base = "C:/Chopan/$user";
$ini = "$base/Properties.ini";
$filesRoot = "$base/Files";

if (!is_dir($base) || !file_exists($ini)) exit;
$props = parse_ini_file($ini);
if (isset($props['locked']) && $props['locked'] == '1') exit;
if (!isset($props['password']) || $props['password'] !== $pass) exit;

$file = realpath($filesRoot . $path);
if (!$file || strpos($file, realpath($filesRoot)) !== 0 || !is_file($file)) exit;

unlink($file);
echo "Success";
?>