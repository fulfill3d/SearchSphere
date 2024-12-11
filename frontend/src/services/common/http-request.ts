export enum HttpMethod {
    GET = 'GET',
    POST = 'POST',
    PUT = 'PUT',
    DELETE = 'DELETE',
    PATCH = 'PATCH'
}

export const httpRequest = async (
    url: string,
    method: HttpMethod = HttpMethod.GET,
    body: any = null,
    headers: { [key: string]: string } = {},
    token?: string
): Promise<any> => {
    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    if (body) {
        body = JSON.stringify(body);
        headers['Content-Type'] = 'application/json';
    }

    try {
        const response = await fetch(url, {
            method,
            body,
            headers,
        });

        const contentType = response.headers.get('Content-Type');
        let data;

        if (contentType && contentType.includes('application/json')) {
            data = await response.json();
        } else {
            data = null;
        }

        if (response.ok) {
            return data;
        } else {
            const errorMessage = data?.message || `Something went wrong: ${response.status}`;
            throw new Error(errorMessage);
        }
    } catch (err: any) {
        throw new Error(err.message);
    }
};
