#pragma once
#include "../../lib/SSTL/sstl.h"

/**
*	The Sensor class is the parent class of every specific sensor added to the system.
*	The purpose of this class is to ensure that every sensor are of the same type, to be able to store all of them in a collection.
*	This is an abstract class, and it forces subclasses to override the read_sensor function as this is different for every sensor, but is yet required.
*	get_name and get_data_count are inherited for every child as they do exactly the same for every type of sensor.
*/
class Sensor
{
public:
	Sensor(const String& name);

	/**
	*	As other classes will inherit this one, we need a virtual destructor to let the compiler know that
	*	a polymorphic object instantiated through a Sensor type might have it's own destructor that has to be called upon deletion.
	*/
	virtual ~Sensor() {};

	/**
	*	Pure virtual read_sensor method to enforce inheriting classes to override this method.
	*/
	virtual void read_sensor() = 0;

	const String& get_name() const;

protected:
	String m_name;
};

/**
*	Constructor setting name.
*/
inline Sensor::Sensor(const String& name)
	: m_name(name) {}

/**
*	Returns a constant string reference to the name member.
*/
inline const String& Sensor::get_name() const
{
	return this->m_name;
}