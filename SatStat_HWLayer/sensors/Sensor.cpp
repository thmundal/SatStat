#pragma once
#include "Sensor.h"

// Constructor
Sensor::Sensor(const String& name, const int& pin, const int& data_count = 1)
{
	this->name = name;
	this->pin = pin;
	this->data_count = data_count;

	Sensor::result = new Result[data_count];
}

// Destructor
Sensor::~Sensor()
{
	delete result;
}

// Returns the sensor name
const String& Sensor::get_name() const
{
	return this->name;
}

// Returns number of readings the sensor provides
const int& Sensor::get_data_count() const
{
	return this->data_count;
}
