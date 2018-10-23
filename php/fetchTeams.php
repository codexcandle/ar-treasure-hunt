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

	$sql = "SELECT id, player_name, level FROM " . $tableName;
	$result = $conn->query($sql);

	if($result->num_rows > 0)
	{
		$players = array();

	    // output data of each row
	    while($row = $result->fetch_assoc())
	    {
	        // echo $row["PlayerName"] . "			LEVEL: " . $row["Level"]. "<br>";

			array_push($players, $row["player_name"]);

	        // echo json_encode($jsonData);
	    }

	    echo json_encode($players);
	}
	else
	{
	    echo "0 results";
	}

	$conn->close();
?>