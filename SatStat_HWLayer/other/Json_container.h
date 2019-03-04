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
	T* get();

protected:
	DynamicJsonBuffer* buffer;
	T* json;
};

template <class T>
inline Json_container<T>::Json_container()
{
	buffer = new DynamicJsonBuffer;
}

template <class T>
inline Json_container<T>::~Json_container()
{
	delete buffer;
}

template<class T>
inline T* Json_container<T>::get()
{
	return json;
}
