using System;

namespace DataStructures {

    class Node<K, V> where K : IComparable<K> {

        // Properties
        public K key;
        public V value;
        public Node<K, V> parent;
        public Node<K, V> leftChild;
        public Node<K, V> rightChild;

        // Constructors
        public Node(K key, V value) {
            this.key = key;
            this.value = value;
            parent = null;
            leftChild = null;
            rightChild = null;
        }

        public Node(K key, V value, Node<K, V> parent) {
            this.key = key;
            this.value = value;
            this.parent = parent;
            leftChild = null;
            rightChild = null;
        }

    }

}
