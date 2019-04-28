#pragma once
#include "Arduino.h"
#include "../../lib/ArduinoJson/ArduinoJson.h"

/**
*	Parent class of Json_object_container and Json_array_container. This class is abstract, and forces every child class to override the create and parse metods.
*	The ArduinoJson library requires JsonObjects and JsonArrays to be created within a JsonBuffer. When a JsonObject or JsonArray is retreived from a buffer, it's returned as a reference. 
*	This means that neither of those can live outside of the buffer, so this class makes sure that a JsonBuffer only contains one JsonObject or JsonArray for it to be easy to keep track
*	of which buffer is related to what object or array when passed around.
*	This class is generic for it to be specified upon instantiation if it's to hold a JsonObject or JsonArray.
*/
template <typename T>
class Json_container
{
public:	
	Json_container();	
	Json_container(const Json_container& src);
	Json_container& operator=(const Json_container& src);
	
	virtual ~Json_container();
	
	T* operator->();

	bool parse(const String& str);
private:
	void copy(const Json_container& src);

	mutable DynamicJsonBuffer* buffer;
	mutable T* json;
};

/**
*	Default constructor. Instantiates the DynamicJsonBuffer and creates an empty object.
*/
template <>
inline Json_container<JsonObject>::Json_container()
{
	buffer = new DynamicJsonBuffer();
	json = &buffer->createObject();
}

/**
*	Default constructor. Instantiates the DynamicJsonBuffer and creates an empty array.
*/
template <>
inline Json_container<JsonArray>::Json_container()
{
	buffer = new DynamicJsonBuffer();
	json = &buffer->createArray();
}

template <>
inline void Json_container<JsonObject>::copy(const Json_container& src)
{
	buffer = new DynamicJsonBuffer();
	String tmp;
	src.json->printTo(tmp);
	json = &buffer->parseObject(tmp);
}

template <>
inline void Json_container<JsonArray>::copy(const Json_container& src)
{
	buffer = new DynamicJsonBuffer();
	String tmp;
	src.json->printTo(tmp);
	json = &buffer->parseArray(tmp);
}

/**
*	Equals operator overload.
*/
template<typename T>
inline Json_container<T>& Json_container<T>::operator=(const Json_container& src)
{	
	delete buffer;		
	copy(src);
	return *this;
}

/**
*	Copy constructor.
*/
template<typename T>
inline Json_container<T>::Json_container(const Json_container& src)
{
	copy(src);	
}

/**
*	Deletes the DynamicJsonBuffer.
*/
template <typename T>
inline Json_container<T>::~Json_container()
{
	delete buffer;
}

/**
*	Returns the generic json data, either a JsonObject or JsonArray depending what the object was instantiated as.
*/
template<typename T>
inline T* Json_container<T>::operator->()
{
	return json;
}

template<>
inline bool Json_container<JsonObject>::parse(const String & str)
{	
	delete buffer;
	buffer = new DynamicJsonBuffer();
	json = &buffer->parseObject(str);

	if (json->success())
	{
		return true;
	}

	return false;
}

template<>
inline bool Json_container<JsonArray>::parse(const String & str)
{
	delete buffer;
	buffer = new DynamicJsonBuffer();
	json = &buffer->parseArray(str);

	if (json->success())
	{
		return true;
	}

	return false;
}
