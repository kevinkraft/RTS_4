Instructions for adding a Resource:
---------------------------------------

1) Make a model
   * Make a game object called, say "Copper".
   * Make a game object as a child called "Model"
   * Add a "Model" strict to "Model"
   * Make the 3D object as children of the model
   * Add a "VisibleRenderer"" script to one of the objects in the model
   * Add a Resource script to the parent "Copper" object.

2) Add its properties to RTS globals
   * Add the N per group, group spread and drop rate attributes of this Resource
   * Add this resource name, "Copper" to the GameTypes.ItemType enum. Also 
     make sure the Resource script from above has this item type.

3) Make an Item prefab for "Copper".