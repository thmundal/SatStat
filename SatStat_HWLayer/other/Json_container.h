#pragma once
#include "Arduino.h"
#include "../libraries/ArduinoJson.h"

template <class T>
class Json_container
{
public:
	Json_container();
	~Json_container();

	virtual void create() = 0;
	virtual void parse(const String &json) = 0;
	virtual T* get() = 0;

protected:
	DynamicJsonBuffer* buffer;
	T* json;
};

template <class T>
Json_container<T>::Json_container()
{
	buffer = new DynamicJsonBuffer;
}

template <class T>
Json_container<T>::~Json_container()
{
	delete buffer;
}