import React, { useState, useEffect } from 'react';
import { getProducts, deleteProduct, updateProduct, createProduct } from '../Api';

const ProductTable = () => {
    const [products, setProducts] = useState([]);
    const [selectedProduct, setSelectedProduct] = useState(null); // Store the selected product for editing
    const [updatedName, setUpdatedName] = useState('');
    const [updatedPrice, setUpdatedPrice] = useState('');
    const [updatedDescription, setUpdatedDescription] = useState('');
    const [isModalOpen, setIsModalOpen] = useState(false); // Manage the modal visibility
    const [isModalCreateOpen, setIsModalCreateOpen] = useState(false);

    // Fetch products when component is mounted
    useEffect(() => {
        fetchProducts();
    }, []);

    const fetchProducts = async () => {
        try {
            const response = await getProducts();
            setProducts(response.data);
        } catch (error) {
            console.error('Error fetching products', error);
        }
    };

    // Handle product delete
    const handleDelete = async (id) => {
        setIsModalOpen(false);
        setIsModalCreateOpen(false); 
        try {
            await deleteProduct(id);
            fetchProducts(); // Refresh the product list
        } catch (error) {
            console.error('Error deleting product', error);
        }
    };

    // Handle product update
    const handleUpdate = (product) => {
        setSelectedProduct(product);
        setUpdatedName(product.name);
        setUpdatedDescription(product.description);
        setUpdatedPrice(product.price);
        setIsModalCreateOpen(false)
        setIsModalOpen(true);
    };

    // Handle product update
    const handleCreate = () => {
        setUpdatedName('');
        setUpdatedDescription('');
        setUpdatedPrice('');
        setIsModalOpen(false);
        setIsModalCreateOpen(true); 
    };

    const handleSaveUpdate = async () => {
        const updatedData = {
            id: selectedProduct.id,
            description: updatedDescription,
            name: updatedName,
            price: updatedPrice,
        };

        try {
            if (selectedProduct) {
                await updateProduct(updatedData);
                fetchProducts(); 
                setIsModalOpen(false); 
            }
        } catch (error) {
            console.error('Error updating product', error);
        }
    };

    const handleSaveCreate = async () => {
        const updatedData = {
            description: updatedDescription,
            name: updatedName,
            price: updatedPrice,
        };

        try {
            await createProduct(updatedData);
            fetchProducts(); 
            setIsModalCreateOpen(false); 
        } catch (error) {
            console.error('Error updating product', error);
        }
    };

    return (
        <div>
            <h3>Product List</h3>
            <button onClick={() => handleCreate()}>Create</button>
            <table>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Price</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {products.map((product) => (
                        <tr key={product.id}>
                            <td>{product.id}</td>
                            <td>{product.name}</td>
                            <td>{product.price}</td>
                            <td>
                                <button onClick={() => handleUpdate(product)}>Update</button>
                                <button onClick={() => handleDelete(product.id)}>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {isModalCreateOpen && (
                <div className="modal">
                    <div className="modal-content">
                        <h2>Create Product</h2>
                        <label>Name</label>
                        <input
                            type="text"
                            value={updatedName}
                            onChange={(e) => setUpdatedName(e.target.value)}
                        />
                        <label>Description</label>
                        <input
                            type="text"
                            value={updatedDescription}
                            onChange={(e) => setUpdatedDescription(e.target.value)}
                        />
                        <label>Price</label>
                        <input
                            type="number"
                            value={updatedPrice}
                            onChange={(e) => setUpdatedPrice(e.target.value)}
                        />
                        <div className="modal-actions">
                            <button onClick={handleSaveCreate}>Create</button>
                            <button onClick={() => setIsModalCreateOpen(false)}>Cancel</button>
                        </div>
                    </div>
                </div>
            )}

            {isModalOpen && (
                <div className="modal">
                    <div className="modal-content">
                        <h2>Update Product</h2>
                        <label>Name</label>
                        <input
                            type="text"
                            value={updatedName}
                            onChange={(e) => setUpdatedName(e.target.value)}
                            disabled
                        />
                        <label>Description</label>
                        <input
                            type="text"
                            value={updatedDescription}
                            onChange={(e) => setUpdatedDescription(e.target.value)}
                        />
                        <label>Price</label>
                        <input
                            type="number"
                            value={updatedPrice}
                            onChange={(e) => setUpdatedPrice(e.target.value)}
                        />
                        <div className="modal-actions">
                            <button onClick={handleSaveUpdate}>Save</button>
                            <button onClick={() => setIsModalOpen(false)}>Cancel</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ProductTable;
