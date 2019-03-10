#pragma once
#include "Sensor.h"

/**
*	Constructor setting name, pin and data_count as well as initializing the Result pointer as an array of size provided by the data_count parameter.
*/
Sensor::Sensor(const String& name, const int& pin, const int& data_count = 1)
{
	this->name = name;
	this->pin = pin;
	this->data_count = data_count;

	Sensor::result = new Result[data_count];
}

/**
*	Destructor deleting the Result pointer.
*/
Sensor::~Sensor()
{
	delete result;
}

/**
*	Returns a constant string reference to the name member.
*/
const String& Sensor::get_name() const
{
	return this->name;
}

/**
*	Returns a constant int reference to the data_count member.
*	data_count represents the number of readings the sensor provides.
*/
const int& Sensor::get_data_count() const
{
	return this->data_count;
}
