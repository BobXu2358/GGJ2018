using UnityEngine;

public class PlayerAction : MonoBehaviour {
    public CharacterType playerType;
    public Transform playerTf;
    public CapsuleCollider2D playerBc;
    public Rigidbody2D playerRb;
    public Transform mapGroundTf;
    public BoxCollider2D mapGroundBc;
    public float moveSpeed;
    public float jumpForce;
    public GameObject Projectile;
    public float fireSpeed;
    public Transform fireOffset;
    bool grounded = true;
    public bool alive = true;
    public GameObject MultiShotBullet;
    public GameObject PiercingBullet;
    public float SprintSpeed;
    public float flashDistance;

    private float realTimeSpeed;
    private GameObject mindBullet;
    private Animator anim;

    private float FadeTime = 0.3f;
    private float ShowTime = 0.3f;
    private Color tempColor = Color.white;
    private float alphaChange;

    [SerializeField] private AudioClip m_JumpSound;
    [SerializeField] private AudioClip m_TransmitSound;
    [SerializeField] private AudioClip m_TransSuccessSound;
    [SerializeField] private AudioClip m_WalkSound;

    void Start()
    {
        realTimeSpeed = moveSpeed;
        anim = GetComponent<Animator>();
        alphaChange = Time.deltaTime / 0.3f;
    }

    void FixedUpdate()
    {
        if (alive)
        {
            if (playerType != CharacterType.None)
            {
                //Control player to move horizontally
                //Debug.Log(realTimeSpeed);
                Vector2 playerSpeed = playerRb.velocity;
                playerSpeed.x = Input.GetAxisRaw("Horizontal") * realTimeSpeed;
                playerRb.velocity = playerSpeed;

                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    this.GetComponent<Animator>().SetBool("isMoving", true);
                }
                else
                {
                    this.GetComponent<Animator>().SetBool("isMoving", false);
                }

                //Control player to jump
                if (grounded && Input.GetButton("Jump"))
                {
                    grounded = false;
                    playerRb.AddForce(new Vector2(0, jumpForce));
                }
            }

            //Make player not move outer of map border
            Vector3 finPlayerPos = playerTf.position;
            if (finPlayerPos.x - playerBc.size.x / 2 <= mapGroundTf.position.x - mapGroundBc.size.x / 2)
                finPlayerPos.x = mapGroundTf.position.x - mapGroundBc.size.x / 2 + playerBc.size.x / 2;
            if (finPlayerPos.x + playerBc.size.x / 2 >= mapGroundTf.position.x + mapGroundBc.size.x / 2)
                finPlayerPos.x = mapGroundTf.position.x + mapGroundBc.size.x / 2 - playerBc.size.x / 2;
            playerTf.position = finPlayerPos;

            //Make player turn back when changing direction
            Vector3 finPlayerScale = playerTf.localScale;
            if (Input.GetAxisRaw("Horizontal") * finPlayerScale.x < 0) finPlayerScale.x *= -1;
            playerTf.localScale = finPlayerScale;

            //Make player fire the brain wave
            if (Input.GetButtonDown("Fire") && mindBullet == null)
            {

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                Vector2 dir = mousePos - new Vector2(fireOffset.position.x, fireOffset.position.y);

                //instantiate bullet
                mindBullet = Instantiate(Projectile, fireOffset.position + new Vector3(0, 0, -5), fireOffset.rotation);

                if (transform.localScale.x == -1)
                    mindBullet.GetComponent<SpriteRenderer>().flipX = true;
                
                 
                dir.Normalize();
                //let it go
                mindBullet.GetComponent<Rigidbody2D>().velocity = dir * fireSpeed;

                PlayTransmitSound();
            }

            if (playerType == CharacterType.Accelerate)
            {
                if (Input.GetButton("Power"))
                {
                    realTimeSpeed = SprintSpeed;
                    Sprint();
                }
                else
                {
                    realTimeSpeed = moveSpeed;
                    anim.SetTrigger("exit");
                }

            }

            if (Input.GetButtonDown("Power"))
            {
                if (playerType == CharacterType.Shoot)
                {
                    Debug.Log("shooting");
                    anim.SetTrigger("shoot");
                }
                    
                if (playerType == CharacterType.Pierce)
                    Pierce(playerTf.localScale.x);
                
                if (playerType == CharacterType.Flash)
                    Flash(playerTf.localScale.x);
            }
            if (Input.GetButtonUp("Power"))
            {
                anim.SetTrigger("exit");
                realTimeSpeed = moveSpeed;
                
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collisionObject){
        if(collisionObject.gameObject.tag == "Bullet"){
            alive = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collisionObject){
        if(collisionObject.gameObject.tag == "Enemy" || collisionObject.gameObject.name == "Map Ground" || collisionObject.gameObject.tag == "Bullet"){
            alive = false;
        }
    //}

    //void OnCollisionStay2D(Collision2D collisionObject){
        //Check if the player is on the ground to jump
        //if(playerTf.position.y - playerBc.size.y / 2 >= collisionObject.gameObject.transform.position.y + collisionObject.gameObject.GetComponent<BoxCollider2D>().size.y / 2){
        if(collisionObject.gameObject.tag == "Obstacle"){
            Vector3 tmpPoint = playerTf.position + new Vector3(-playerBc.size.x / 3, -playerBc.size.y / 2, 0);
            RaycastHit2D hit = Physics2D.Raycast(tmpPoint, new Vector2(0, -1));
            //Debug.Log(Mathf.Abs(tmpPoint.y - hit.point.y));
            //Debug.DrawLine(tmpPoint,hit.point);
            if(hit.collider != null){
                if(hit.collider.gameObject.tag == "Obstacle" && Mathf.Abs(tmpPoint.y - hit.point.y) <= 0.05f){
                    grounded = true;
                }
            }
            tmpPoint = playerTf.position + new Vector3(playerBc.size.x / 3, -playerBc.size.y / 2, 0);
            hit = Physics2D.Raycast(tmpPoint, new Vector2(0, -1));
            //Debug.Log(Mathf.Abs(tmpPoint.y - hit.point.y));
            //Debug.DrawLine(tmpPoint,hit.point);
            if(hit.collider != null){
                if(hit.collider.gameObject.tag == "Obstacle" && Mathf.Abs(tmpPoint.y - hit.point.y) <= 0.05f){
                    grounded = true;
                }
            }
        }
        //}
    }

    void PlayJumpSound()
    {
        this.GetComponent<AudioSource>().clip = m_JumpSound;
        this.GetComponent<AudioSource>().Play() ;
    }

    void PlayMoveSound()
    {
        ;
    }

    public void PlayTransmitSuccessSound()
    {
        this.GetComponent<AudioSource>().clip = m_TransSuccessSound;
        this.GetComponent<AudioSource>().Play();
    }

    public void PlayTransmitSound()
    {
        this.GetComponent<AudioSource>().clip = m_TransmitSound;
        this.GetComponent<AudioSource>().Play();
    }

    void Shoot()
    {
        
        GameObject bullet0 = Instantiate(MultiShotBullet, fireOffset.position, fireOffset.rotation);
        GameObject bullet1 = Instantiate(MultiShotBullet, fireOffset.position, Quaternion.Euler(0, 0, 45));
        GameObject bullet2 = Instantiate(MultiShotBullet, fireOffset.position, Quaternion.Euler(0, 0, -45));

        Vector2 dir1 = Quaternion.Euler(0, 0, 45) * Vector2.right;
        Vector2 dir2 = Quaternion.Euler(0, 0, -45) * Vector2.right;
        //let it go
        if (transform.localScale.x == 1)
        {
            bullet0.GetComponent<Rigidbody2D>().velocity = transform.right * fireSpeed;
            bullet1.GetComponent<Rigidbody2D>().velocity = dir1 * fireSpeed;
            bullet2.GetComponent<Rigidbody2D>().velocity = dir2 * fireSpeed;
        }

        if (transform.localScale.x == -1)
        {
            bullet0.GetComponent<SpriteRenderer>().flipX = true;
            bullet1.GetComponent<SpriteRenderer>().flipX = true;
            bullet2.GetComponent<SpriteRenderer>().flipX = true;

            bullet0.GetComponent<Rigidbody2D>().velocity = -transform.right * fireSpeed;
            bullet1.GetComponent<Rigidbody2D>().velocity = -dir1 * fireSpeed;
            bullet2.GetComponent<Rigidbody2D>().velocity = -dir2 * fireSpeed;
        }
    }

    void Pierce(float facing)
    {
        anim.SetTrigger("shoot");
        GameObject bullet = Instantiate(PiercingBullet, fireOffset.position, fireOffset.rotation);
        //let it go
        if (facing == 1)
            bullet.GetComponent<Rigidbody2D>().velocity = transform.right * fireSpeed;
        if (facing == -1)
        {
            bullet.GetComponent<SpriteRenderer>().flipX = true;
            bullet.GetComponent<Rigidbody2D>().velocity = -transform.right * fireSpeed;
        }
            
    }

    void Sprint()
    {
        anim.SetTrigger("start");
        realTimeSpeed = SprintSpeed;
    }

    void Flash(float facing)
    {

        anim.SetTrigger("flash");

        float x = transform.position.x;


        if (facing == 1)
        {
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward);
            //if(hit.collider!= null && hit.distance > flashDistance) 
                transform.position = new Vector3(x + flashDistance, transform.position.y, transform.position.z);
            //else
            //    transform.position = new Vector3(x + hit.distance, transform.position.y, transform.position.z);
        }

        else
        {
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.forward);
            //if (hit.collider != null && hit.distance > flashDistance)
                transform.position = new Vector3(x - flashDistance, transform.position.y, transform.position.z);
            //else
                //transform.position = new Vector3(x - hit.distance, transform.position.y, transform.position.z);
        }

    }
}
