import axios from "axios";

export async function getCategories() {
    try {
        const reponse = await
        axios.get(`https://localhost:7044/api/categories?PageSize=10&PageNumber=1`);
        const data = reponse.data;
        if (data.isSuccess) {
            return data.result;
            console.log(data.result)
        }
        else{
            return null;
            
        }
    } catch (error){
        console.log('Error', error.message);
        return null;
    }
}

export async function getFeatured(PageSize= 10 , PageNumber=1) {
    try {
        const reponse = await
        axios.get(`https://localhost:7044/api/posts/featured/2`);
        const data = reponse.data;
        if (data.isSuccess) {
            return data.result;
           // console.log(data.result)
        }
        else{
            return null;
            
        }
    } catch (error){
        console.log('Error', error.message);
        return null;
    }
}

    export async function getRandom(PageSize= 10 , PageNumber=1) {
        try {
            const reponse = await
            axios.get(`https://localhost:7044/api/posts/random/2`);
            const data = reponse.data;
            if (data.isSuccess) {
                return data.result;
              //  console.log(data.result)
            }
            else{
                return null;
                
            }
        } catch (error){
            console.log('Error', error.message);
            return null;
        }
    }

        export async function getTagCloud(PageSize= 10 , PageNumber=1) {
            try {
                const reponse = await
                axios.get(`https://localhost:7044/api/posts/tagcloud`);
                const data = reponse.data;
                if (data.isSuccess) {
                    return data.result;
                   // console.log(data.result)
                }
                else{
                    return null;
                    
                }
            } catch (error){
                console.log('Error', error.message);
                return null;
            }
    }
