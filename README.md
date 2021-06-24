# Connect Four

This project was designed for mobile platforms. Build generated and tested for Android.

## Features

-Vertical orientation;
-Responsive UI, including script to handle safe areas;
-Basic animations for the Selection Arrow and End Game Popup. Both made with Animator component;
-Player input is mouse-based. Press and hold to see the selection arrow. Move left or right to choose another column. Release it to select a column.
-Button to reset the game anytime;
-EndGame popup. Click anywhere to close it and reset the game; 
-Complete game loop;

## Considerations

I'm using 2 third party plugins (both are free): 
	-SafeArea: it's a single script that manages mobile devices safe areas
	-DOTween: nice plugin for animations. I use it in almost all of my projects since 2016. Very useful and productive. In this project I used it to animate the checkers

Tests were made using Debug.Log and VS debug step by step.

