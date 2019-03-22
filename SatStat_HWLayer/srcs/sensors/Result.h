#pragma once
#include "Arduino.h"

/**
*	Result is a strcut containing the name of a specific kind of data a sensor can read, as well as the read data.
*	The reason for this struct is that one sensor might be able to measure more than one thing, so by adding multiple
*	Result objects to that sensor would make it possible to name the different measurements, and contain the read values in a sensible way.
*/
struct Result
{
public:
	/**
	* Constructer with initializer list. Initializing the subscribe_to member.
	*/
	Result()
		: subscribed_to(false) {};

	// Getters
	const String& get_name() const { return name; };
	const int& get_data() const { return data; };
	const bool& is_subscribed_to() const { return subscribed_to; };

	// Setters
	void set_name(const String& name) { this->name = name; };
	void set_data(const int& data) { this->data = data; };
	void subscribe() { this->subscribed_to = true; };
	void unsubscribe() { this->subscribed_to = false; };

private:
	String name;
	int data;
	bool subscribed_to;
};