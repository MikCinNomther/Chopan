<?php
// 參數獲取
$user = $_GET['username'] ?? '';
$pass = $_GET['password'] ?? '';
$dirPath = urldecode($_GET['DirectoryPath'] ?? '');
$base = "C:/Chopan/$user";
$ini = "$base/Properties.ini";
$filesRoot = "$base/Files";

// 驗證用戶
if (!is_dir($base) || !file_exists($ini)) {
    http_response_code(403);
    echo "UserError";
    exit;
}
$props = parse_ini_file($ini);
if (isset($props['locked']) && $props['locked'] == '1') {
    http_response_code(403);
    echo "Locked";
    exit;
}
if (!isset($props['password']) || $props['password'] !== $pass) {
    http_response_code(403);
    echo "PasswordError";
    exit;
}

// 目標目錄安全檢查
$targetRoot = realpath($filesRoot);
if ($targetRoot === false) {
    http_response_code(500);
    echo "ServerError";
    exit;
}
$createDir = $targetRoot . '/' . ltrim(str_replace('\\', '/', $dirPath), '/');

// 防止目錄穿越
if (strpos(realpath(dirname($createDir)) ?: dirname($createDir), $targetRoot) !== 0) {
    http_response_code(403);
    echo "PathError";
    exit;
}

// 建立目錄
if (is_dir($createDir)) {
    echo "AlreadyExists";
    exit;
}
if (mkdir($createDir, 0777, true)) {
    echo "Success";
} else {
    http_response_code(500);
    echo "CreateError";
}
?>