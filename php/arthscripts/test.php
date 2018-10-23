<?php
	switch($_GET['f'])
	{
		case 'treasureFunction':
	    	treasureFunction();
	    	break;
	    default:
	    	// ...	
	}

	echo "ARTreasureHunts are fun!";

	function treasureFunction()
	{
		echo "************************ treasure function hit!";

		return 999;
	}
?>