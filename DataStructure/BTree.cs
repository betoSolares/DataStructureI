using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructure {

    public class BTree<V, K> where K : IComparable<K> {

        // Properties
        public int Count { get; private set; }
        private Node<K, V> root;
        private int order;

        // Constructor
        public BTree(int order) {
            Count = 0;
            root = new Node<K, V>(order);
            this.order = order;
        }

        // Insert an element in the tree
        public void Insert(K key, V value) {
            if (root.IsFull) {
                Node<K, V> newChilds = root;
                root = new Node<K, V>(order);
                root.childs.Add(newChilds);
                SplitNode(root, newChilds, 0);
                InsertElement(root, key, value);
            } else {
                InsertElement(root, key, value);
            }
            Count++;
        }

        // Remove an element in the tree
        public void Remove(K key) {
            Delete(root, key);
            if (root.elements.Count == 0 && !root.IsLeaf) {
                root = root.childs.Single();
            }
        }

        // Find and return the value of the node
        public V Find(K key) {
            Element<K, V> element = Search(root, key);
            if (element == null) {
                return default(V);
            } else {
                return element.value;
            }
        }

        // Clear the tree
        public void Clear() {
            root = new Node<K, V>(order);

        }

        // Return the tree in form of a list
        public List<V> ToList() {
            List<V> list = new List<V>();
            ConvertList(root, ref list);
            return list;
        }

        // Update the value of an element in the tree
        public void Update(K key, V value) {
            if(root.elements.Count > 0) {
                Update(root, key, value);
            }
        }

        // Check if the tree is empty
        public bool IsEmpty() {
            if (root.elements.Count == 0)
                return true;
            else
                return false;
        }

        /**
         * @desc: Split a node that is full into two nodes.
         * @param: Node<K, V> parentNode - The parent node of the split node.
         * @param: Node<K,V> splitNode - The node to be splited.
         * @param: int index - Index to be splited.
        **/
        private void SplitNode(Node<K, V> parentNode, Node<K, V> split, int index) {
            Node<K, V> newNode = new Node<K, V>(order);
            parentNode.elements.Insert(index, split.elements[order - 1]);
            parentNode.childs.Insert(index + 1, newNode);
            newNode.elements.AddRange(split.elements.GetRange(order, order - 1));
            split.elements.RemoveRange(order - 1, order);
            if (!split.IsLeaf) {
                newNode.childs.AddRange(split.childs.GetRange(order, order));
                split.childs.RemoveRange(order, order);
            }
        }

        /**
         * @desc: Insert a new element in the node.
         * @param: Node<K, V> currentNode - The current node in the tree.
         * @param: K key - The new key.
         * @param: V value - The new value.
        **/
        private void InsertElement(Node<K, V> currentNode, K key, V value) {
            int index = currentNode.elements.TakeWhile(e => key.CompareTo(e.key) >= 0).Count();
            if (currentNode.IsLeaf) {
                currentNode.elements.Insert(index, new Element<K, V>(key, value));
            } else {
                Node<K, V> child = currentNode.childs[index];
                if (child.IsFull) {
                    SplitNode(currentNode, child, index);
                    if (key.CompareTo(currentNode.elements[index].key) > 0) {
                        index++;
                    }
                }
                InsertElement(currentNode.childs[index], key, value);
            }
        }

        /**
         * @desc: Delete an element in the tree.
         * @param: Node<K, V>currentNode - The current node in the tree.
         * @param: K key - The key to delete.
        **/
        private void Delete(Node<K, V> currentNode, K key) {
            int index = currentNode.elements.TakeWhile(e => key.CompareTo(e.key) > 0).Count();
            if (index < currentNode.elements.Count && currentNode.elements[index].key.CompareTo(key) == 0) {
                DeleteFromNode(currentNode, key, index);
            } else {
                if (!currentNode.IsLeaf) {
                    DeleteFromSub(currentNode, key, index);
                }
            }
        }

        /**
         * @desc: deletes key from a node that contains it, be this node a leaf node or an internal node.
         * @param: Node<K,V> node - Node that contains the key.
         * @param: K key - The value of the key.
         * @param: int index - The index of the key.
        **/
        private void DeleteFromNode(Node<K, V> node, K key, int index) {
            if (node.IsLeaf) {
                node.elements.RemoveAt(index);
                Count--;
            } else {
                Node<K, V> predecessorChild = node.childs[index];
                if (predecessorChild.elements.Count >= order) {
                    Element<K, V> predecessor = DeletePredecessor(predecessorChild);
                    node.elements[index] = predecessor;
                } else {
                    Node<K, V> successorChild = node.childs[index + 1];
                    if (successorChild.elements.Count >= order) {
                        Element<K, V> successor = DeleteSuccessor(predecessorChild);
                        node.elements[index] = successor;
                    } else {
                        predecessorChild.elements.Add(node.elements[index]);
                        predecessorChild.elements.AddRange(successorChild.elements);
                        predecessorChild.childs.AddRange(successorChild.childs);
                        node.elements.RemoveAt(index);
                        node.childs.RemoveAt(index + 1);
                        Delete(predecessorChild, key);
                    }
                }
            }
        }

        /**
         * @desc: Delete a an element that is in a sub tree
         * @param: Node<K, V> parentNode - The parent node.
         * @param: K key - The key to delete.
         * @param: int index - The index of the key.
        **/
        private void DeleteFromSub(Node<K, V> parentNode, K key, int index) {
            Node<K, V> childNode = parentNode.childs[index];
            if (childNode.HasMinimum) {
                int leftIndex = index - 1;
                Node<K, V> leftSibling = null;
                if (index > 0) {
                    leftSibling = parentNode.childs[leftIndex];
                }
                int rightIndex = index + 1;
                Node<K, V> rightSibling = null;
                if (index < parentNode.childs.Count - 1) {
                    rightSibling = parentNode.childs[rightIndex];
                }
                if (leftSibling != null && leftSibling.elements.Count > order - 1) {
                    childNode.elements.Insert(0, parentNode.elements[index]);
                    parentNode.elements[index] = leftSibling.elements.Last();
                    leftSibling.elements.RemoveAt(leftSibling.elements.Count - 1);
                    if (!leftSibling.IsLeaf) {
                        childNode.childs.Insert(0, leftSibling.childs.Last());
                        leftSibling.childs.RemoveAt(leftSibling.childs.Count - 1);
                    }
                } else if (rightSibling != null && rightSibling.elements.Count > order - 1) {
                    childNode.elements.Add(parentNode.elements[index]);
                    parentNode.elements[index] = rightSibling.elements.First();
                    rightSibling.elements.RemoveAt(0);
                    if (!rightSibling.IsLeaf) {
                        childNode.childs.Add(rightSibling.childs.First());
                        rightSibling.childs.RemoveAt(0);
                    }
                } else {
                    if (leftSibling != null) {
                        childNode.elements.Insert(0, parentNode.elements[index]);
                        List<Element<K, V>> oldEntries = childNode.elements;
                        childNode.elements = leftSibling.elements;
                        childNode.elements.AddRange(oldEntries);
                        if (!leftSibling.IsLeaf) {
                            List<Node<K, V>> oldChildren = childNode.childs;
                            childNode.childs = leftSibling.childs;
                            childNode.childs.AddRange(oldChildren);
                        }
                        parentNode.childs.RemoveAt(leftIndex);
                        parentNode.elements.RemoveAt(index);
                    } else {
                        childNode.elements.Add(parentNode.elements[index]);
                        childNode.elements.AddRange(rightSibling.elements);
                        if (!rightSibling.IsLeaf) {
                            childNode.childs.AddRange(rightSibling.childs);
                        }
                        parentNode.childs.RemoveAt(rightIndex);
                        parentNode.elements.RemoveAt(index);
                    }
                }
            }
            Delete(childNode, key);
        }

        /**
         * @desc: Delete the predecessor of a node.
         * @para: Node<K, V> The node to delete the predecessor.
        **/
        private Element<K, V> DeletePredecessor(Node<K, V> node) {
            if (node.IsLeaf) {
                 Element<K, V> result = node.elements[node.elements.Count - 1];
                node.elements.RemoveAt(node.elements.Count - 1);
                return result;
            }
            return DeletePredecessor(node.childs.Last());
        }

        /**
         * @desc: Delete the sucessor of a node.
         * @para: Node<K, V> The node to delete the sucessor.
        **/
        private Element<K, V> DeleteSuccessor(Node<K, V> node) {
            if (node.IsLeaf) {
                Element<K, V> result = node.elements[0];
                node.elements.RemoveAt(0);
                return result;
            }
            return DeletePredecessor(node.childs.First());
        }

        /**
         * @desc: Find an element in the tree.
         * @param: Node<K, V> node - The node to search the key.
         * @param: K key - The key to search.
         * @return: Elemen<K, V> / null - If find the key/ if don't find the key.
        **/
        private Element<K, V> Search(Node<K, V> node, K key) {
            int index = node.elements.TakeWhile(entry => key.CompareTo(entry.key) > 0).Count();
            if (index < node.elements.Count && node.elements[index].key.CompareTo(key) == 0) {
                return node.elements[index];
            } else {
                if(node.IsLeaf) {
                    return null;
                } else {
                    return Search(node.childs[index], key);
                }
            }
        }

        /**
         * @desc: Convert the tree to a list.
         * @param: Node<K, V> node - The node to start the iteration.
         * @param: ref List<V> list - The list to return.
        **/
        private void ConvertList(Node<K, V> node, ref List<V> list) {
            if(node.elements.Count > 0) {
                if(node.childs.Count > 0) {
                    for(int i = 0; i < node.childs.Count; i++) {
                        ConvertList(node.childs[i], ref list);
                    }
                }
                for(int i = 0; i < node.elements.Count; i++) {
                    list.Add(node.elements[i].value);
                }
            }
        }

        /**
         * @desc: Update the value of and element in the tree.
         * @param: Node<K,V> node - The node to start iterate.
         * @param: K key - The key to find.
         * @param: V value - The new value.
        **/
        private void Update(Node<K, V> node, K key, V value) {
            int index = node.elements.TakeWhile(entry => key.CompareTo(entry.key) > 0).Count();
            if (index < node.elements.Count && node.elements[index].key.CompareTo(key) == 0) {
                node.elements[index] = new Element<K, V>(key, value);
            } else {
                if (!node.IsLeaf) {
                    Update(node.childs[index], key, value);
                }
            }
        }

    }

}
