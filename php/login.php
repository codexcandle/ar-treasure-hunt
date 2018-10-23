<?php
	// define('MIN_VALUE', '0.0'); 

	$conn = null;

	switch($_GET['f'])
	{
		case 'login':
	    	login();
	    	break;
	    default:
	    	// ...	
	}

	function connect()
	{
		if($conn != null)
			return;

		// create connection
		$conn = new mysqli($servername, $username, $password, $dbname);
		if($conn->connect_error)
		{
	    	die("Connection failed: " . $conn->connect_error);
		}
	}

	function login()
	{
		// connect to database
		// TODO - figure out why below function call fails (TP20160721)
		// connect();
		//////////////////////////////////////////
		$servername = "mysql.fancy-sql-server.com";
		$username = "fancy-user";
		$password = "fancy-pass";
		$dbname = "db_treasurehunt";
		$tableName = "GameTable";
		$levelTable = "Levels";

		// create connection
		$conn = new mysqli($servername, $username, $password, $dbname);
		if($conn->connect_error)
		{
	    	die("Connection failed: " . $conn->connect_error);
		}
		//////////////////////////////////////////

		$teamIndex = $_REQUEST["teamIndex"];
		$idVal = ($teamIndex + 1);

		$sql = "SELECT level FROM " . $gameTable . " WHERE id=" . $idVal;

		// parse results
		$result = $conn->query($sql);

		if($result->num_rows > 0)
		{
			$results = array();
			$players = array();

		    // output data of each row
		    while($row = $result->fetch_assoc())
		    {
		        // echo $row["PlayerName"] . "			LEVEL: " . $row["Level"]. "<br>";

				array_push($players, $row["Level"]);

				// if this is 1st time logging in, promote them to level 1! (default = 0)
				$playerLevelNum = -1;
				if($row["level"] == 0)
				{	
					$newLevelNum = 1;
					$sql = "UPDATE " . $gameTable . " SET level=" . $newLevelNum . " WHERE id=" . $idVal;
					if($conn->query($sql) === FALSE)
					{
					    echo "Error updating record: " . $conn->error;

					    return;
					}

					$playerLevelNum = $newLevelNum;
				}
				else
				{
					$playerLevelNum = $row["level"];
				}

/*
$obj = {};
$obj['clue']
*/

				// echo $playerLevelNum;

//////////////////////////////////////////////////////////////////////
// gather "level-related" data
$sql = "SELECT id, prompt, target, clue FROM " . $levelTable . " WHERE id=" . $playerLevelNum;

		// parse results
		$result = $conn->query($sql);

		if($result->num_rows > 0)
		{
			$results = array();
			$players = array();

		    // output data of each row
		    while($row = $result->fetch_assoc())
		    {
		        // echo $row["PlayerName"] . "			LEVEL: " . $row["Level"]. "<br>";

				array_push($players, $row);
			}
		}
///////////////////////////////////////////////////////////////////////
		    }
echo json_encode($players);
		}
		else
		{
		    echo "0 results";
		}
	}
?>