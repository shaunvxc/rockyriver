
using System;

public interface ICanoeController
{

	 void paddleLeft();
	 void paddleRight();

	 bool isLocked();
	 void HaltAllMovement();


	 bool isMoving();

	 void haltRiverForceMovement();
}


