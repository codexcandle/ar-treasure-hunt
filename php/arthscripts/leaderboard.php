<!Doctype html>
<head>
<title>AR Treasure Hunt Leaderboard</title>
<link rel ="stylesheet" type ="text/css" href="sqltest.css">
</head>
<body>
<h1>AR Treasure Hunt Leaderboard</h1>
</body>
</html>

<?php
	$servername = "mysql.fancy-sql-server.com";
	$username = "fancy-user";
	$password = "fancy-pass";
	$dbname = "db_treasurehunt";
	$tableName = "GameTable";

	// create connection
	$conn = new mysqli($servername, $username, $password, $dbname);
	if($conn->connect_error)
	{
	    die("Connection failed: " . $conn->connect_error);
	} 

	$sql = "SELECT ID, PlayerName, Level FROM " . $tableName;
	$result = $conn->query($sql);

	if($result->num_rows > 0)
	{
	    // output data of each row
	    while($row = $result->fetch_assoc())
	    {
	        echo $row["PlayerName"] . " LEVEL: " . $row["Level"]. "<br>";
	    }
	}
	else
	{
	    echo "0 results";
	}

	$conn->close();
?>