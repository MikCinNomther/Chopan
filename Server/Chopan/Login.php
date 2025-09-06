<?php
$user = $_GET['username'] ?? '';
$pass = $_GET['password'] ?? '';
$base = "C:/Chopan/$user";
$ini = "$base/Properties.ini";

if (!is_dir($base) || !file_exists($ini)) {
    echo "UserError";
    exit;
}
$props = parse_ini_file($ini);
if (isset($props['locked']) && $props['locked'] == '1') {
    echo "Locked";
    exit;
}
if (!isset($props['password']) || $props['password'] !== $pass) {
    echo "PasswordError";
    exit;
}
echo "Success";
?>