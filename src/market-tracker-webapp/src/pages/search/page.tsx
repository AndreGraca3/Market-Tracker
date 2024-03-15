interface SearchPageQuery {
  m: string; // brand
  c: string; // category
  loja: string; // store group
}

export default function SearchPage({
  searchParams,
}: {
  searchParams: SearchPageQuery;
}) {
  return <div>Search Page {JSON.stringify(searchParams)}</div>;
}
