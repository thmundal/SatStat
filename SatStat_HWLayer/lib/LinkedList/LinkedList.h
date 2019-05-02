#pragma once
#include "Node.h"

/**
*	LinkedList is a generic linked list class implementation.
*	It contains the first Node in the list, and has methods to access and modify theese nodes.
*/
template <class KEY, class VALUE>
class LinkedList
{
public:
	// Default constructor
	LinkedList();

	// Copy constructor
	LinkedList(const LinkedList<KEY, VALUE>& other);

	// Destructor
	~LinkedList();

	// Assignment operator overload
	LinkedList<KEY, VALUE>& operator=(const LinkedList<KEY, VALUE>& other);

	// Subscript member access operator overloads	
	VALUE& operator[](int index);
	VALUE& operator[](const KEY& key);

	// Add empty
	void add(const KEY& key);

	// Append value
	void append(const KEY& key, const VALUE& value);

	// Get value
	VALUE& get(const KEY& key);

	// Set value of existing
	void set(const KEY& key, const VALUE& value);

	// Erase object
	void erase(const KEY& key);

	// Set default value
	void setDefault(const VALUE& value);

	// Number of objects
	const int& count() const;	

	// Contains key
	bool contains(const KEY& key) const;

private:
	// First node
	Node<KEY, VALUE>* m_next;

	// Default value
	VALUE m_default;

	// Number of objects
	int m_count;
};

/**
*	Default constructor. Setting m_next to nullptr and m_count to zero.
*/
template<class KEY, class VALUE>
inline LinkedList<KEY, VALUE>::LinkedList()
{
	m_next = nullptr;
	m_count = 0;
}

/**
*	Copy constructor. Using assignment operator to perform a deep copy from other to this.
*/
template<class KEY, class VALUE>
inline LinkedList<KEY, VALUE>::LinkedList(const LinkedList<KEY, VALUE>& other)
{	
	*this = other;
}

/**
*	Destructor. Deleting m_next which will call Node's destructor recursively deleting every Node in the list.
*/
template<class KEY, class VALUE>
inline LinkedList<KEY, VALUE>::~LinkedList()
{
	if (m_next != nullptr)
	{
		delete m_next;
	}
}

/**
*	Assignment operator overload. Deletes m_next if not nullptr, and performes a deep copy from other to this.
*	Returns a reference to this to allow chaining.
*/
template<class KEY, class VALUE>
inline LinkedList<KEY, VALUE>& LinkedList<KEY, VALUE>::operator=(const LinkedList<KEY, VALUE>& other)
{
	if (m_next != nullptr)
	{
		delete m_next;
	}

	m_default = other.m_default;
	m_count = other.m_count;

	if (other.m_next != nullptr)
	{
		m_next = new Node<KEY, VALUE>;
		*m_next = *other.m_next;
	}

	return *this;
}

/**
*	Subscript member access operator overload. Returns the value at the given index.
*	Returns default value if index out of bounds.
*/
template<class KEY, class VALUE>
inline VALUE& LinkedList<KEY, VALUE>::operator[](int index)
{
	if (m_count > 0 && index < m_count)
	{
		return m_next->get_at_index(index);
	}

	return m_default;
}

/**
*	Subscript member access operator overload. Returns the result of the get method.
*/
template<class KEY, class VALUE>
inline VALUE& LinkedList<KEY, VALUE>::operator[](const KEY& key)
{
	return get(key);
}

/**
*	Adds an entry to the list with no predefined value.
*/
template<class KEY, class VALUE>
inline void LinkedList<KEY, VALUE>::add(const KEY& key)
{
	if (m_next == nullptr)
	{
		m_next = new Node<KEY, VALUE>(key);
	}
	else
	{
		m_next->add(key);
	}
	m_count++;
}

/**
*	Appends an entry to the list with predefined value.
*/
template<class KEY, class VALUE>
inline void LinkedList<KEY, VALUE>::append(const KEY& key, const VALUE& value)
{
	if (m_next == nullptr)
	{
		m_next = new Node<KEY,VALUE>(key, value);
	}
	else
	{
		m_next->append(key, value);
	}
	m_count++;
}

/**
*	Looks for an object with the given key, returns it's value if found, returns default value if not.
*/
template<class KEY, class VALUE>
inline VALUE& LinkedList<KEY, VALUE>::get(const KEY& key)
{
	if (m_next != nullptr)
	{
		return m_next->get(key, m_default);
	}
	return m_default;
}

/**
*	Sets the value of an existing object. Appends a new object if no object with the given already key exists.
*/
template<class KEY, class VALUE>
inline void LinkedList<KEY, VALUE>::set(const KEY& key, const VALUE& value)
{
	if (m_next != nullptr)
	{
		if (m_next->set(key, value))
		{
			m_count++;
		}
	}
}

/**
*	Erases an object from the list.
*/
template<class KEY, class VALUE>
inline void LinkedList<KEY, VALUE>::erase(const KEY& key)
{	
	if (m_next != nullptr)
	{
		if (m_next->erase(key, m_next))
		{
			m_count--;
		}
	}
}

/**
*	Sets the default value.
*/
template<class KEY, class VALUE>
inline void LinkedList<KEY, VALUE>::setDefault(const VALUE& value)
{
	m_default = value;
}

/**
*	Returns the number of objects in the list.
*/
template<class KEY, class VALUE>
inline const int& LinkedList<KEY, VALUE>::count() const
{
	return m_count;
}

/**
*	Checks if an object with the given key is present in the list. Returns true if it is, false if not.
*/
template<class KEY, class VALUE>
inline bool LinkedList<KEY, VALUE>::contains(const KEY& key) const
{
	if (m_next != nullptr)
	{
		return m_next->contains(key);
	}
	return false;
}