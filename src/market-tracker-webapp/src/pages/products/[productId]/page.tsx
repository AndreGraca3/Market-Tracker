interface ProductPageProps {
  productId: string;
}

export default function ProductPage({ params }: { params: ProductPageProps }) {
  return <div>Product Page id: {params.productId}</div>;
}
