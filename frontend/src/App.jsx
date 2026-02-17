import { Admin, Resource, ListGuesser, EditGuesser, Layout } from "react-admin";
import simpleRestProvider from "ra-data-simple-rest";
import { useRealtime } from "./realtimeDataProvider";

const dataProvider = simpleRestProvider("http://localhost:5284/api");

// Custom layout that enables realtime updates
const RealtimeLayout = ({ children, ...props }) => {
  useRealtime();
  return <Layout {...props}>{children}</Layout>;
};

export default function App() {
  return (
    <Admin dataProvider={dataProvider} layout={RealtimeLayout}>
      <Resource
        name="products"
        list={ListGuesser}
        edit={EditGuesser}
      />
    </Admin>
  );
}