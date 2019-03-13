using System;
using System.Collections.Generic;

namespace DataStructures {

    public class BST<V, K> where K : IComparable<K> {

        // Properties
        private Node<K, V> root;
        public int Count { get; private set; }

        // Constructor
        public BST() {
            root = null;
            Count = 0;
        }

        // Insert a new elemetn in the tree
        public void Insert(K key, V value) {
            if(root == null) {
                root = new Node<K, V>(key, value);
            } else {
                root = Insert(key, value, root);
            }
            Count++;
        }

        // Delete the node with the key
        public void Remove(K key) {
            root = Delete(key, root);
            Count--;
        }

        // Find and return the value of the node
        public V Find(K key) {
            Node<K, V> node = Find(root, key);
            if(node == null) {
                return default(V);
            } else {
                return node.value;
            }
        }

        // Clear the tree
        public void Clear() {
            root = null;
        }

        // Return the tree in a sorted list
        public List<V> ToList() {
            List<V> list = new List<V>();
            InOrder(root, ref list);
            return list;
        }

        // Update the element in the tree
        public void Update(K key, V value) {
            if(root != null) {
                root = Update(root, key, value);
            }
        }

        // Check if the tree is empty
        public bool IsEmpty() {
            if (root == null)
                return true;
            else
                return false;
        }

        /**
         * @desc: Find the place to add the new item.
         * @param: K key - The value to compare in the tree.
         * @param: V value - Data of the new node in the tree.
         * @param: Node<K, V> currentNode - the current node in the tree.
         * @return: Node<K, V> - The new structure of the tree.
        **/
        private Node<K, V> Insert(K key, V value, Node<K, V> currentNode) {
            if(currentNode == null) {
                currentNode = new Node<K, V>(key, value);
                return currentNode;
            } else if(key.CompareTo(currentNode.key) <= 0) {
                currentNode.leftChild = Insert(key, value, currentNode.leftChild);
            } else {
                currentNode.rightChild = Insert(key, value, currentNode.rightChild);
            }
            return currentNode;
        }

        /**
         * @desc: Find and delete a item in the tree.
         * @param: K key - The value to compare in the tree.
         * @param: V value - Data of the new node in the tree.
         * @param: Node<K, V> currentNode - the current node in the tree.
         * @return: Node<K, V> - The new structure of the tree.
        **/
        private Node<K, V> Delete(K key, Node<K, V> currentNode) {
            if(currentNode == null) {
                return null;
            } else {
                if(key.CompareTo(currentNode.key) < 0) {
                    currentNode.leftChild = Delete(key, currentNode.leftChild);
                } else if(key.CompareTo(currentNode.key) > 0) {
                    currentNode.rightChild = Delete(key, currentNode.rightChild);
                } else {
                    if(currentNode.leftChild == null && currentNode.rightChild == null) {
                        currentNode = null;
                        return currentNode;
                    } else if(currentNode.rightChild == null) {
                        currentNode = currentNode.leftChild;
                    } else if(currentNode.leftChild == null) {
                        currentNode = currentNode.rightChild;
                    } else {
                        Node<K, V> minValue = FindMinimun(currentNode.rightChild);
                        currentNode.key = minValue.key;
                        currentNode.value = minValue.value;
                        currentNode.rightChild = Delete(minValue.key, currentNode.rightChild);
                    }
                }
            }
            return currentNode;
        }

        // Find the minimun value in the subtree
        private Node<K, V> FindMinimun(Node<K, V> currentNode) {
            if(currentNode == null) {
                return null;
            } else if(currentNode.leftChild == null) {
                return currentNode;
            }
            return FindMinimun(currentNode.leftChild);
        }

        /**
         * @desc: Find and return the value of the node.
         * @param: K key - The value to compare in the tree.
         * @param: Node<K, V> currentNode - the current node in the tree.
         * @return: V - The value of the node.
        **/
        private Node<K, V> Find(Node<K, V> currentNode, K key) {
            if(currentNode == null || currentNode.key.CompareTo(key) == 0) {
                return currentNode;
            }
            if(key.CompareTo(currentNode.key) > 0) {
                return Find(currentNode.rightChild, key);
            }
            return Find(currentNode.leftChild, key);
        }

        /**
         * @desc: Iterate the tree and add each item in the list.
         * @param: Node<K, V> currentNode - the current node in the tree.
         * @param: ref List<V> list - The reference to the list.
        **/
        private void InOrder(Node<K, V> currentNode, ref List<V> list) {
            if(currentNode != null) {
                InOrder(currentNode.leftChild, ref list);
                list.Add(currentNode.value);
                InOrder(currentNode.rightChild, ref list);
            }
        }

        /**
         * @desc: Find the node to update.
         * @param: Node<K, V> currentNode - the current node in the tree.
         * @param: K key - The value to compare in the tree.
         * @param: V value - Data to update.
         * @return: Node<K, V> - The new structure of the tree.
        **/
        private Node<K, V> Update(Node<K, V> currentNode, K key, V value) {
            if (currentNode.key.CompareTo(key) == 0) {
                currentNode.value = value;
                return currentNode;
            } else if (key.CompareTo(currentNode.key) <= 0) {
                currentNode.leftChild = Update(currentNode.leftChild, key, value);
            } else {
                currentNode.rightChild = Update(currentNode.rightChild, key, value);
            }
            return currentNode;
        }

    }

}
