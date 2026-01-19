import React, { useEffect, useState } from "react";
import type { School } from "../types/school";


function SchoolList() {
  const [schools, setSchools] = useState<School[]>([]);

  useEffect(() => {
    // Fetch school data from API or other source
    const fetchSchools = async () => {
      const response = await fetch("/api/school");
      const data = await response.json();
      setSchools(data);
    };

    fetchSchools();
  }, []);

    return (
        <div className="container">
            <h1>School List</h1>
            <table>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Address</th>
                        <th>Student Count</th>
                    </tr>
                </thead>
                <tbody>
                    {schools.length > 0 ? (
                        schools.map((school) => (
                            <tr key={school.id}>
                                <td>{school.id}</td>
                                <td>{school.name}</td>
                                <td>{school.address}</td>
                                <td>{school.studentCount}</td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan={4}>Loading schools or no data found...</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
}

export default SchoolList;
