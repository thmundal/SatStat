#pragma once
#include "Arduino.h"

/**
*	Result is a strcut containing the name of a specific kind of data a sensor can read, as well as the read data.
*	The reason for this struct is that one sensor might be able to measure more than one thing, so by adding multiple
*	Result objects to that sensor would make it possible to name the different measurements, and contain the read values in a sensible way.
*/
struct Result
{
	String name;
	int data;
};