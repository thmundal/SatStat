#pragma once
#include "../../libraries/SSTL/sstl.h"
#include "../../libraries/LinkedList.h"

/**
*	The Sensor class is the parent class of every specific sensor added to the system.
*	The purpose of this class is to ensure that every sensor are of the same type, to be able to store all of them in a collection.
*	This is an abstract class, and it foreces subclasses to override the read_sensor function as this is different for every sensor, but is yet required.
*	get_name and get_data_count are inherited for every child as they do exactly the same for every type of sensor.
*/
class Sensor
{
public:
	Sensor(const String& name, const int& pin);
	virtual ~Sensor() {};

	virtual const sub_list_type& read_sensor() = 0;

	const String& get_name() const;

protected:
	String name;
	int pin;
};