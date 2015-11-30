package com.filip.androidgames.framework;

import java.util.ArrayList;
import java.util.List;

// This is a generically typed class, much like collection 
// classes such as ArrayList. 
// Generics allow us to store any type o object in our Pool 
// without having to cast continuously
public class Pool<T> 
{
	
	// This pool object factory interface will be used
	// to create any type of objects that we want
    public interface PoolObjectFactory<T> 
    {
        public T createObject();
    }

    // store pooled objects
    private final List<T> freeObjects;
    
    // used to generate new instances of the type held by the class
    private final PoolObjectFactory<T> factory;
    
    // stores the maximum number of objects the Pool can hold
    private final int maxSize;

    public Pool(PoolObjectFactory<T> factory, int maxSize) {
        this.factory = factory;
        this.maxSize = maxSize;
        this.freeObjects = new ArrayList<T>(maxSize);
    }

    // responsible for either handing us a brand-new instance of the type held by the Pool, 
    // via the PoolObjectFactory.newObject() method, 
    // or returning a pooled instance in case there’s one in the freeObjectsArrayList
    public T newObject() {
        T object = null;

        if (freeObjects.size() == 0)
            object = factory.createObject();
        else
            object = freeObjects.remove(freeObjects.size() - 1);

        return object;
    }

    // Lets us reinsert objects that we no longer use.
    // If the list is full, the object is not added, and it is likely to be 
    // consumed by the garbage collector the next time it executes.
    public void free(T object) {
        if (freeObjects.size() < maxSize)
            freeObjects.add(object);
    }
}
