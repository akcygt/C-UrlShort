<?php

// Veritabanına bağlantı kurun
$servername = "your_servername";
$username = "your_username";
$password = "your_password";
$dbname = "your_dbname";

$conn = new mysqli($servername, $username, $password, $dbname);
if ($conn->connect_error) {
    die("Veritabanına bağlantı kurulamadı: " . $conn->connect_error);
}

// Kısaltılmış linki alın
$a = $_GET["a"];

// Kısaltılmış linki kullanarak veritabanından orijinal linki alın
$sql = "SELECT original_link FROM links WHERE s = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $a);
$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    // Veritabanında kısaltılmış link ile ilişkili olan bir kayıt bulundu
    $row = $result->fetch_assoc();
    $originalLink = $row["original_link"];

    // Tarayıcıda orijinal linki açın
    header("Location: $originalLink");
} else {
    // Veritabanında kısaltılmış link ile ilişkili olan bir kayıt bulunamadı
    http_response_code(404);
    echo "The requested URL was not found on this server.";
}

$conn->close();

?>