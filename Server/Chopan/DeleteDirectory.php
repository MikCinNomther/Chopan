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
$delDir = realpath($filesRoot . $dirPath);
if ($delDir === false || strpos($delDir, $targetRoot) !== 0 || !is_dir($delDir)) {
    http_response_code(404);
    echo "NotFound";
    exit;
}

// 遞歸刪除目錄
function rrmdir($dir) {
    foreach (array_diff(scandir($dir), ['.','..']) as $file) {
        $path = "$dir/$file";
        if (is_dir($path)) {
            rrmdir($path);
        } else {
            unlink($path);
        }
    }
    rmdir($dir);
}

try {
    rrmdir($delDir);
    echo "Success";
} catch (Exception $e) {
    http_response_code(500);
    echo "DeleteError";
}
?>