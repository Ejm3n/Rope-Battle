При использовании данного пакета необходимо 
1. Выкинуть на сцену в абсолютно любое место префаб LevelTransition
2. В нужный момент, при нажатии на кнопку Next или Restart вызывать 
	LevelTransitionEffect.Default.DoTransition(Action onComplete);
3. В аргумент передается код, которые обычно вызывается сразу по нажатии на кнопку, к примеру
	LevelTransitionEffect.Default.DoTransition(() => 
		{
			UIManager.Default.State = UIManagerState.Start;
			LevelManager.Default.Restart();
		});