#pragma once

/**
*	Node is a generic class implementation for the nodes contained in a linked list.
*	This class is complementary to the LinkedList class, and contains the key-value pairs.
*/
template <class KEY, class VALUE>
class Node
{
public:
	// Constructors
	Node();
	Node(const KEY & key);
	Node(const KEY & key, const VALUE & val);
	Node(const Node<KEY, VALUE> &other);

	// Destructor
	~Node();

	// Assignment operator overload
	Node<KEY, VALUE>& operator=(const Node<KEY, VALUE>& other);

	// Add empty
	void add(const KEY& key);

	// Append value
	void append(const KEY& key, const VALUE& value);

	// Getters
	VALUE& get_at_index(int& index);
	VALUE& get(const KEY& key, VALUE& default_value);

	// Setter
	bool set(const KEY& key, const VALUE& value);

	// Erase object
	bool erase(const KEY& key, Node*& parent_m_next);

	// Contains key
	bool contains(const KEY& key) const;

private:
	Node* m_next;
	KEY m_key;
	VALUE m_value;
};

/**
*	Default constructor. Setting m_next to nullptr.
*/
template<class KEY, class VALUE>
inline Node<KEY, VALUE>::Node()
{
	m_next = nullptr;
}

/**
*	Constructor. Setting m_next to nullptr, and m_key to the given key.
*/
template<class KEY, class VALUE>
inline Node<KEY, VALUE>::Node(const KEY& key)
{
	m_next = nullptr;
	m_key = key;
}

/**
*	Constructor. Sets m_next to nullptr, and sets both the key and value.
*/
template<class KEY, class VALUE>
inline Node<KEY, VALUE>::Node(const KEY& key, const VALUE& val)
{
	m_next = nullptr;
	m_key = key;
	m_value = val;
}

/**
*	Copy constructor. Using assignment operator to perform a deep copy from other to this.
*/
template<class KEY, class VALUE>
inline Node<KEY, VALUE>::Node(const Node<KEY, VALUE>& other)
{	
	*this = other;
}

/**
*	Destructor. Recursively deleting every node in the list.
*/
template<class KEY, class VALUE>
inline Node<KEY, VALUE>::~Node()
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
inline Node<KEY, VALUE>& Node<KEY, VALUE>::operator=(const Node<KEY, VALUE>& other)
{

	if (m_next != nullptr)
	{
		delete m_next;
	}

	m_key = other.m_key;
	m_value = other.m_value;

	if (other.m_next != nullptr)
	{
		m_next = new Node<KEY, VALUE>;
		*m_next = *other.m_next;
	}

	return *this;
}

/**
*	Adds an entry to the list with no predefined value.
*/
template<class KEY, class VALUE>
inline void Node<KEY, VALUE>::add(const KEY& key)
{
	if (m_next == nullptr)
	{
		m_next = new Node(key);
	}
	else
	{
		m_next->add(key);
	}
}

/**
*	Appends an entry to the list with predefined value.
*/
template<class KEY, class VALUE>
inline void Node<KEY, VALUE>::append(const KEY& key, const VALUE& value)
{
	if (m_next == nullptr)
	{
		m_next = new Node(key, value);
	}
	else
	{
		m_next->append(key, value);
	}
}

/**
*	Iterates through the objects in the list and decrement the index for each element.
*	Returns the value of the current object when the index hits zero.
*/
template<class KEY, class VALUE>
inline VALUE& Node<KEY, VALUE>::get_at_index(int& index)
{
	if (index == 0)
	{
		return m_value;
	}
	return m_next->get_at_index(--index);	
}

/**
*	Looks for an object with the given key, returns it's value if found, returns default value if not.
*/
template<class KEY, class VALUE>
inline VALUE& Node<KEY, VALUE>::get(const KEY& key, VALUE& default_value)
{
	if (m_key == key)
	{
		return m_value;
	}
	if (m_next != nullptr)
	{
		return m_next->get(key, default_value);
	}
	return default_value;
}

/**
*	Sets the value of an existing object. Appends a new object if no object with the given key already exists.
*/
template<class KEY, class VALUE>
inline bool Node<KEY, VALUE>::set(const KEY& key, const VALUE& value)
{
	if (m_key == key)
	{
		m_value = value;
	}
	else if (m_next != nullptr)
	{
		return m_next->set(key, value);
	}
	else
	{
		append(key, value);
		return true;
	}
	return false;
}

/**
*	Erases an object from the list.
*/
template<class KEY, class VALUE>
inline bool Node<KEY, VALUE>::erase(const KEY& key, Node*& parent_m_next)
{
	if (m_key == key)
	{
		parent_m_next = m_next;
		m_next = nullptr;
		delete this;
		return true;
	}
	else if (m_next != nullptr)
	{
		return m_next->erase(key, m_next);
	}
	return false;
}

/**
*	Checks if an object with the given key is present in the list. Returns true if it is, false if not.
*/
template<class KEY, class VALUE>
inline bool Node<KEY, VALUE>::contains(const KEY& key) const
{
	if (m_key == key)
	{
		return true;
	}
	if (m_next != nullptr)
	{
		return m_next->contains(key);
	}
	return false;
}