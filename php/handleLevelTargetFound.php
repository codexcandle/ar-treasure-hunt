<?php
	// define('MIN_VALUE', '0.0'); 

	$conn = null;

	switch($_GET['f'])
	{
		case 'handleLevelTargetFound':
	    	handleLevelTargetFound();
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

	function handleLevelTargetFound()
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

				// promote to next level
				$newLevelNum = $row["level"] + 1;

				// get "level" count ///////////////////////
				$sql = "SELECT id FROM " . $levelTable;

				// parse results
				$result = $conn->query($sql);

				$levelCount = $result->num_rows;			
				////////////////////////////////////////////

				if(newLevelNum <= $levelCount)
				{
					$sql = "UPDATE " . $gameTable . " SET level=" . $newLevelNum . " WHERE id=" . $idVal;
					if($conn->query($sql) === FALSE)
					{
					    echo "Error updating record: " . $conn->error;

					    return;
					}

					// gather "level-related" data
					$sql = "SELECT id, prompt, target, clue FROM " . $levelTable . " WHERE id=" . $newLevelNum;

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

							// update game "won" status
							$row['won'] = ($row['level'] >= $levelCount);

							array_push($players, $row);

							echo json_encode($players);
						}
					}
				}
				else
				{
					/*
					$endGameData = array();
					$endGameData['won'] = true;
					echo json_encode($endGameData);
					*/
					
					echo "endGame!";
				}			
			}
		}
	}	
?>