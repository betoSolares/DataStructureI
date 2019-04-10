using System;
using System.Collections.Generic;

namespace DataStructure {

    class Node<K, V> where K : IComparable<K> {

        // Properties
        public List<Element<K, V>> elements;
        public List<Node<K, V>> childs;
        private int order;

        // Constructor
        public Node(int order) {
            elements = new List<Element<K, V>>(order);
            childs = new List<Node<K, V>>(order);
            this.order = order;
        }

        // Determinate if the node is a leaf
        public bool IsLeaf => childs.Count == 0;

        // Verify if the node has the minimum number of elements
        public bool HasMinimum => elements.Count == order - 1;

        // Verify if the node is full
        public bool IsFull => elements.Count == (2 * order) - 1;

    }

}
