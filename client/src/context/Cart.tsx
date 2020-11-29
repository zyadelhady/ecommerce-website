import React, {
  FC,
  createContext,
  useState,
  useContext,
  useCallback,
} from 'react';
import { ErrorContext } from './Error';
import axios from '../axios';

interface ICartContext {
  cart: ICart;
  isLoading: boolean;
  getCart: () => Promise<void>;
  addToCart: (data: IAddToCart) => Promise<void>;
  removeFromCart: (id: string) => Promise<void>;
  updateCartItem: (
    id: string,
    index: number,
    data: IUpdateCartItem
  ) => Promise<void>;
}

export interface ICartItem {
  id: string;
  color: string;
  size: string;
  quantity: number;
  product: IProductItem;
}

interface IProductItem {
  quantity: number;
  name: string;
  price: string;
  image: string;
}

interface ICart {
  items: ICartItem[];
  totalPrice: number;
  length: number;
}

interface IAddToCart {
  productId: string;
  color: string;
  size: string;
  quantity: number;
}

interface IUpdateCartItem {
  quantity: number;
}

export const CartContext = createContext<ICartContext>({
  cart: { items: [], totalPrice: 0, length: 0 },
  isLoading: false,
  getCart: async () => {},
  addToCart: async () => {},
  removeFromCart: async () => {},
  updateCartItem: async () => {},
});

export const CartProvider: FC = (props) => {
  const [cart, setCart] = useState<ICartContext['cart']>({
    items: [],
    totalPrice: 0,
    length: 0,
  });
  const [isLoading, setIsLoading] = useState<ICartContext['isLoading']>(false);

  const { setError } = useContext(ErrorContext);

  const getCart = useCallback(async () => {
    setIsLoading(true);
    try {
      const res = await axios.get('/cart');
      setCart(res.data);
      console.log(res.data);
      console.log(cart);
    } catch (error) {
      setError(error.response.data.message);
    } finally {
      setIsLoading(false);
    }
  }, [setError]);

  const addToCart = async (data: IAddToCart) => {
    console.log(data);
    if (!data.color || !data.size || !data.productId) {
      return setError('Cart item not valid.');
    }
    setIsLoading(true);
    try {
      await axios.post('/cart', data);
    } catch (error) {
      setError(error.response.data.message);
    } finally {
      setIsLoading(false);
    }
  };

  const removeFromCart = async (id: string) => {
    setIsLoading(true);
    try {
      await axios.delete(`/cart/${id}`);
      setCart(({ items, totalPrice, length }) => {
        const newItems = items.filter((item) => {
          if (item.id === id) {
            totalPrice -= item.quantity * +item.product.price;
            return false;
          }
          return true;
        });

        return {
          items: newItems,
          length: length - 1,
          totalPrice,
        };
      });
    } catch (error) {
      setError(error.response.data.message);
    } finally {
      setIsLoading(false);
    }
  };

  const updateCartItem = async (
    id: string,
    index: number,
    data: IUpdateCartItem
  ) => {
    if (data.quantity < 1) {
      return setError('Quantity must be at least 1.');
    }
    setIsLoading(true);
    try {
      await axios.put(`/cart/item/${id}`, data);
      const item = cart.items[index];
      setCart((prev) => {
        prev.totalPrice -= item.quantity * +item.product.price;
        prev.totalPrice += data.quantity * +item.product.price;
        item.quantity = data.quantity;
        return { ...prev };
      });
    } catch (error) {
      setError(error.response.data.message);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <CartContext.Provider
      value={{
        cart,
        isLoading,
        getCart,
        addToCart,
        removeFromCart,
        updateCartItem,
      }}
    >
      {props.children}
    </CartContext.Provider>
  );
};
