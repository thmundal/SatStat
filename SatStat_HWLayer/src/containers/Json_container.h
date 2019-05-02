#pragma once
#include "Arduino.h"
#include "../../lib/ArduinoJson/ArduinoJson.h"

/**
*	The ArduinoJson library requires JsonObjects and JsonArrays to be created within a JsonBuffer.
*	When a JsonObject or JsonArray is retreived from a buffer, it's returned as a reference. 
*	This means that neither of those can live outside of the buffer,
*	so this class encapsulates the JsonBuffer together with a JsonObject or JsonArray to simplify object creation and passing.
*	This class is generic for it to be specified upon instantiation if it's to hold a JsonObject or JsonArray.
*/
template <typename T>
class Json_container
{
public:	
	// Default constructor
	Json_container();	

	// Copy constructor
	Json_container(const Json_container& src);

	// Assignment operator overload
	Json_container& operator=(const Json_container& src);
	
	// Destructor
	~Json_container();
	
	// Arrow operator overload
	T* operator->();

	// Parse and get methods
	bool parse(const String& str);
	T& get();

private:
	// Copy method used in copy constructor and assignment operator
	void copy(const Json_container& src);

	// Member variables.
	DynamicJsonBuffer* m_buffer;
	T* m_json;
};

/**
*	Default constructor. Instantiates the DynamicJsonBuffer and creates an empty object.
*/
template <>
inline Json_container<JsonObject>::Json_container()
{
	m_buffer = new DynamicJsonBuffer();
	m_json = &m_buffer->createObject();
}

/**
*	Default constructor. Instantiates the DynamicJsonBuffer and creates an empty array.
*/
template <>
inline Json_container<JsonArray>::Json_container()
{
	m_buffer = new DynamicJsonBuffer();
	m_json = &m_buffer->createArray();
}

/**
*	Performes a deep copy from the source object to this object.
*/
template <>
inline void Json_container<JsonObject>::copy(const Json_container& src)
{
	m_buffer = new DynamicJsonBuffer();
	String tmp;
	src.m_json->printTo(tmp);
	m_json = &m_buffer->parseObject(tmp);
}

/**
*	Performes a deep copy from the source object to this object.
*/
template <>
inline void Json_container<JsonArray>::copy(const Json_container& src)
{
	m_buffer = new DynamicJsonBuffer();
	String tmp;
	src.m_json->printTo(tmp);
	m_json = &m_buffer->parseArray(tmp);
}

/**
*	Copy constructor. Calls the copy method to perform a deep copy.
*/
template<typename T>
inline Json_container<T>::Json_container(const Json_container& src)
{
	copy(src);	
}

/**
*	Assignment operator overload. Utilizes the copy method to perform a deep copy.
*/
template<typename T>
inline Json_container<T>& Json_container<T>::operator=(const Json_container& src)
{	
	m_buffer->clear();
	delete m_buffer;		
	copy(src);
	return *this;
}

/**
*	Destructor. Clears and deletes the DynamicJsonBuffer.
*/
template <typename T>
inline Json_container<T>::~Json_container()
{
	m_buffer->clear();
	delete m_buffer;
}

/**
*	Returns a pointer to the JSON data.
*	Allow the user to access the public members of m_json directly without a getter.
*/
template<typename T>
inline T* Json_container<T>::operator->()
{
	return m_json;
}

/**
*	Parses a string to the JsonObject.
*/
template<>
inline bool Json_container<JsonObject>::parse(const String& str)
{
	m_buffer->clear();
	m_json = &m_buffer->parseObject(str);

	if (m_json->success())
	{
		return true;
	}

	return false;
}

/**
*	Parses a string to the JsonArray.
*/
template<>
inline bool Json_container<JsonArray>::parse(const String& str)
{
	m_buffer->clear();
	m_json = &m_buffer->parseArray(str);

	if (m_json->success())
	{
		return true;
	}

	return false;
}

/**
*	Returns a reference to the JSON data.
*/
template<typename T>
inline T& Json_container<T>::get()
{
	return *m_json;
}
