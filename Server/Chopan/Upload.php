<?php
// 參數獲取
$user = $_POST['username'] ?? '';
$pass = $_POST['password'] ?? '';
$savePath = $_POST['SavePath'] ?? '';
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

// 處理檔案
if (!isset($_FILES['Data'])) {
    http_response_code(400);
    echo "NoFile";
    exit;
}

// 目標路徑安全檢查
$target = realpath($filesRoot);
if ($target === false) {
    http_response_code(500);
    echo "ServerError";
    exit;
}
$saveFullPath = $target . '/' . ltrim(str_replace('\\', '/', $savePath), '/');
$saveDir = dirname($saveFullPath);

// 防止目錄穿越
if (strpos(realpath($saveDir) ?: $saveDir, $target) !== 0) {
    http_response_code(403);
    echo "PathError";
    exit;
}

// 建立目錄（如不存在）
if (!is_dir($saveDir)) {
    mkdir($saveDir, 0777, true);
}

// 覆蓋寫入檔案
if (move_uploaded_file($_FILES['Data']['tmp_name'], $saveFullPath)) {
    echo "Success";
} else {
    http_response_code(500);
    echo "SaveError";
}
?>