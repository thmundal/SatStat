#pragma once
#include "Arduino.h"
#include "../../libraries/ArduinoJson.h"

/**
*	Parent class of Json_object_container and Json_array_container. This class is abstract, and forces every child class to override the create and parse metods.
*	The ArduinoJson library requires JsonObjects and JsonArrays to be created within a JsonBuffer. When a JsonObject or JsonArray is retreived from a buffer, it's returned as a reference. 
*	This means that neither of those can live outside of the buffer, so this class makes sure that a JsonBuffer only contains one JsonObject or JsonArray for it to be easy to keep track
*	of which buffer is related to what object or array when passed around.
*	This class is generic for it to be specified upon instantiation if it's to hold a JsonObject or JsonArray.
*/
template <class T>
class Json_container
{
public:
	Json_container();
	virtual ~Json_container();

	virtual void create() = 0;
	virtual bool parse(const String &json) = 0;
	T& get() const;

protected:
	DynamicJsonBuffer* buffer;
	T* json;
};

/**
*	Instantiating the DynamicJsonBuffer.
*/
template <class T>
inline Json_container<T>::Json_container()
{
	buffer = new DynamicJsonBuffer;
}

/**
*	Deletes the DynamicJsonBuffer.
*/
template <class T>
inline Json_container<T>::~Json_container()
{
	delete buffer;
}

/**
*	Returns the generic json data, either a JsonObject or JsonArray depending what the object was instantiated as.
*/
template<class T>
inline T& Json_container<T>::get() const
{
	return *json;
}
